using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VIPP.Models;
using VIPP.Hubs;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace VIPP.Controllers
{
	public class HomeController : Controller
	{
		static ApplicationDbContext _context = ApplicationDbContext.Create();

		public async Task<ActionResult> Index()
		{
			if (ModelState.IsValid)
			{
				var userIdentity = User.Identity;
				if(userIdentity.IsAuthenticated == true)
				{
					string userId = User.Identity.GetUserId();
					DateTime currDate = DateTime.Now;
					List<MarathonLink> marathonLinks = new List<MarathonLink>();
					List<string> upcomingMarathons = new List<string>();
					using (var _context = ApplicationDbContext.Create())
					{
						IEnumerable<Participant> marathonsForCurrUser = await _context.Participants.Where(p => p.UserId == userId).ToListAsync();
						foreach (Participant marathonForCurrUser in marathonsForCurrUser)
						{
							try
							{
								MarathonDate marathonDate = await _context.MarathonDates.FindAsync(marathonForCurrUser.MarathonDateId);
								if (marathonDate != null)
								{
									Guid marathonId = marathonDate.MarathonId;
									DateTime startDate = marathonDate.StartDate;
									Marathon marathon = await _context.Marathons.FindAsync(marathonId);
									if (marathon != null)
									{
										int countDays = marathon.CountDays;
										DateTime dateAfterEnd = startDate.AddDays(countDays);
										if (currDate < dateAfterEnd)
										{
											if (currDate < startDate)
											{
												upcomingMarathons.Add($"{marathon.Name}, который стартует {startDate}");
											}
											else
											{
												marathonLinks.Add(new MarathonLink { LinkText = $"{marathon.Name}, который стартовал {startDate}", ActionName = "AddAchievement", Day = currDate.Subtract(startDate).Days + 1, CurrUserId = userId });
											}
										}
									}
								}
							}
							catch (InvalidOperationException e)
							{
								Console.WriteLine(e.Message);
							}
						}
						ViewBag.MarathonLinks = marathonLinks;
						ViewBag.UpcomingMarathons = upcomingMarathons;
						if (User.IsInRole("admin"))
						{
							ViewBag.MarathonSeeAchievementsLinks = await indexForAdmin(_context);
						}
					}
				}
			}
			return View();
		}

		[HttpGet]
		private async Task<List<MarathonLink>> indexForAdmin(ApplicationDbContext _context)
		{
			List<MarathonLink> marathonLinks = new List<MarathonLink>();
			DateTime currDate = DateTime.Now;
			foreach (var marathonDate in _context.MarathonDates.ToList())
			{
				try
				{
					Guid marathonId = marathonDate.MarathonId;
					DateTime startDate = marathonDate.StartDate;
					Marathon marathon = await _context.Marathons.FindAsync(marathonId);
					if (marathon != null)
					{
						int countDays = marathon.CountDays;
						DateTime dateAfterEnd = startDate.AddDays(countDays);
						if (currDate < dateAfterEnd && currDate >= startDate)
						{
							marathonLinks.Add(new MarathonLink { LinkText = $"{marathon.Name}, который стартовал {startDate}. Просмотреть успехи.", ActionName = "SeeAchievements", Day = currDate.Subtract(startDate).Days + 1, MarathonDateId = marathonDate.Id });
						}
					}
				}
				catch(Exception exc)
				{
					HttpContext.Response.Write(exc);
				}
			}
			return marathonLinks;
		}

		[HttpGet]
		public async Task<ActionResult> AddAchievement(int activeDay, int currDay, string userId)
		{
			ViewBag.ActiveDay = activeDay;
			ViewBag.CurrDay = currDay;
			ViewBag.UserId = userId;
			using(var _context = ApplicationDbContext.Create())
			{
				ViewBag.currUserAchievements = await _context.Achievements.Where(ach => ach.UserId == userId && ach.Day == activeDay).ToListAsync();
				//var resume = await _context.Resumes.Where(r => r.UserId == userId && r.Day == activeDay).FirstOrDefaultAsync();

				var (id, resume) = GetResume(userId, activeDay, _context);
				ViewBag.Id = id;
				ViewBag.Resume = resume;

				//var feedback = await _context.Feedbacks.Where(r => r.UserId == userId && r.Day == activeDay).FirstOrDefaultAsync();
				//if(feedback != nullSignalR
				//{
				//	ViewBag.Feedback = feedback.Feedback;
				//}

				var (id_, feedbackText) = GetFeedback(userId, activeDay, _context);
				ViewBag.Feedback = feedbackText;

				if (activeDay == 7)
				{
					var (finalId, finalFeedbackText) = GetFeedback(userId, 8, _context);
					ViewBag.FinalFeedback = finalFeedbackText;
				}
			}
			return View();
		}

		[HttpPost]
		public async Task<JsonResult> AddAchievement(string userId, int day, string achievement)
		{
			Guid achievementId = Guid.NewGuid();
			SelfEstimationCheckList selfEstimationCheckList = new SelfEstimationCheckList { Id = achievementId, UserId = userId, Day = day, Achievement = achievement };
			using(var _context = ApplicationDbContext.Create())
			{
				if (ModelState.IsValid)
				{
					_context.Achievements.Add(selfEstimationCheckList);
				}
				await _context.SaveChangesAsync();
			}
			return Json(selfEstimationCheckList);
		}

		[HttpPost]
		public async Task<ActionResult> EditAchievement(string userId, int day, string achievement, string? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			Guid achievementId = Guid.Parse(id);
			SelfEstimationCheckList selfEstimationCheckList = new SelfEstimationCheckList { Id = achievementId, UserId = userId, Day = day, Achievement = achievement};
			using (var _context = ApplicationDbContext.Create())
			{
				_context.Entry(selfEstimationCheckList).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<ActionResult> SendResume(string? id, string userId, int day, string resume)
		{
			using(var _context = ApplicationDbContext.Create())
			{
				try
				{
					if (id == "")
					{
						SelfEstimationResumeFromUser selfEstimationResumeFromUser = new SelfEstimationResumeFromUser { Id = Guid.NewGuid(), UserId = userId, Day = day, Resume = resume };
						if (ModelState.IsValid)
						{
							_context.Resumes.Add(selfEstimationResumeFromUser);
						}
						await _context.SaveChangesAsync();
						return Json(selfEstimationResumeFromUser);
					}
					else
					{
						Guid id_ = Guid.Parse(id);
						SelfEstimationResumeFromUser selfEstimationResumeFromUser = await _context.Resumes.FindAsync(id_);
						selfEstimationResumeFromUser.Resume = resume;
						_context.Entry(selfEstimationResumeFromUser).State = EntityState.Modified;
						await _context.SaveChangesAsync();
					}
				}
				catch (Exception exc)
				{
					HttpContext.Response.Write(exc.Message);
				}
			}
			return Json(null);
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public ActionResult SeeAchievements(int currDay, Guid marathonDateId)
		{
			using (var _context = ApplicationDbContext.Create())
			{
				List<UserDone> usersDone = new List<UserDone>();
				var participants = _context.Participants.Where(p => p.MarathonDateId == marathonDateId).ToList();
				var currUsers = new List<ApplicationUser>();
				foreach(var participant in participants)
				{
					var userId = participant.UserId;
					var user = _context.Users.Find(userId);
					string SQLRqstAchievements = "select count(*) from SelfEstimationCheckLists where UserId = N'" + userId + "' AND Day = " + currDay;
					string SQLRqstResume = "select count(*) from SelfEstimationResumeFromUsers where UserId = N'" + userId + "' AND Day = " + currDay;
					int completeAch = _context.Database.SqlQuery<int>(SQLRqstAchievements).First();
					int completeR = _context.Database.SqlQuery<int>(SQLRqstResume).First();
					bool complete = (completeAch == 10 && completeR == 1) ? true : false;
					if(complete)
					{
						usersDone.Add(new UserDone { UserId = user.Id, UserName = user.UserName });
					}
				}
				ViewBag.UsersDone = usersDone;
			}
			ViewBag.CurrDay = currDay;
			return View();
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public ActionResult SeeAchievementsOfUser(int activeDay, int currDay, string userId, string name)
		{
			using (var _context = ApplicationDbContext.Create())
			{ 
				ViewBag.ActiveDay = activeDay;
				ViewBag.CurrDay = currDay;
				ViewBag.UserId = userId;
				ViewBag.Name = name;
				if(activeDay == currDay)
				{
					List<SelfEstimationCheckList> achievements = _context.Achievements.Where(ach => ach.UserId == userId && ach.Day == currDay).ToList();
					ViewBag.Achievements = achievements;
				}

				var resume = _context.Resumes.Where(r => r.UserId == userId && r.Day == activeDay).FirstOrDefault();
				if(resume != null)
				{
					ViewBag.Resume = resume.Resume;
				}

				var (id, feedback) = GetFeedback(userId, activeDay, _context);
				ViewBag.Id = id;
				ViewBag.Feedback = feedback;

				var (finalId, finalFeedback) = GetFeedback(userId, 8, _context);
				ViewBag.FinalId = finalId;
				ViewBag.FinalFeedback = finalFeedback;
			}
			return View();
		}

		private (Guid, string) GetFeedback(string userId, int day, ApplicationDbContext _context)
		{
			var feedback = _context.Feedbacks.Where(r => r.UserId == userId && r.Day == day).FirstOrDefault();
			var result = (id: Guid.Empty, feedback: "");
			if (feedback != null)
			{
				result.id = feedback.Id;
				result.feedback = feedback.Feedback;
			}
			return result;
		}

		private (Guid, string) GetResume(string userId, int day, ApplicationDbContext _context)
		{
			var resume = _context.Resumes.Where(r => r.UserId == userId && r.Day == day).FirstOrDefault();
			var result = (id: Guid.Empty, resume: "");
			if (resume != null)
			{
				result.id = resume.Id;
				result.resume = resume.Resume;
			}
			return result;
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public async Task<JsonResult> SeeAchievementsOfUser(int day, string userId, string feedback, string? id)
		{
			using(var _context = ApplicationDbContext.Create())
			{
				try
				{
					if (id == "")
					{
						SelfEstimationFeedbackToUser selfEstimationFeedbackToUser = new SelfEstimationFeedbackToUser { Id = Guid.NewGuid(), UserId = userId, Day = day, Feedback = feedback };
						_context.Feedbacks.Add(selfEstimationFeedbackToUser);
						await _context.SaveChangesAsync();
						return Json(selfEstimationFeedbackToUser);
					}
					else
					{
						Guid id_ = Guid.Parse(id);
						SelfEstimationFeedbackToUser selfEstimationFeedbackToUser = await _context.Feedbacks.FindAsync(id_);
						selfEstimationFeedbackToUser.Feedback = feedback;
						_context.Entry(selfEstimationFeedbackToUser).State = EntityState.Modified;
						await _context.SaveChangesAsync();
					}
					var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();

				}
				catch (Exception exc)
				{
					HttpContext.Response.Write(exc.Message);
				}
			}
			return Json(null);
		}
	}
}