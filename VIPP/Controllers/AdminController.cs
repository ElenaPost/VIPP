using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VIPP.Models;
using System.Data.Entity;

namespace VIPP.Controllers
{
	public class AdminController : Controller
	{

		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Index()
		{
			if (ModelState.IsValid)
			{
				using(var _context = ApplicationDbContext.Create())
				{
					var users = await _context.Users.ToListAsync();
					return View(users);
				}
			}
			return View();
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public ActionResult AddMarathon()
		{
			return PartialView();
		}

		[HttpPost]
		public async Task<ActionResult> AddMarathon(Marathon marathon)
		{
			if(ModelState.IsValid)
			{
				using (var _context = ApplicationDbContext.Create())
				{
					marathon.Id = Guid.NewGuid();
					_context.Marathons.Add(marathon);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> SetMarathonDate()
		{
			using (var _context = ApplicationDbContext.Create())
			{
				ViewBag.Marathons = new SelectList(await _context.Marathons.ToListAsync(), "Id", "Name");
			}
			return PartialView();
		}

		[HttpPost]
		public async Task<ActionResult> SetMarathonDate(MarathonDate _marathonDate)
		{
			if(ModelState.IsValid)
			{
				using (var _context = ApplicationDbContext.Create())
				{
					var id = Guid.NewGuid();
					DateTime date = new DateTime(_marathonDate.Year, _marathonDate.Month, _marathonDate.Day);
					MarathonDate marathonDate = new MarathonDate { Id = id, MarathonId = _marathonDate.MarathonId, StartDate = date };
					_context.MarathonDates.Add(marathonDate);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> AddParticipants()
		{
			using (var _context = ApplicationDbContext.Create())
			{
				ViewBag.MarathonDates = new SelectList(await _context.MarathonDates.ToListAsync(), "Id", "StartDate");
				ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
			}
			return PartialView();
		}

		[HttpPost]
		public async Task<ActionResult> AddParticipants(Participant _participant)
		{
			if(ModelState.IsValid)
			{
				using (var _context = ApplicationDbContext.Create())
				{
					Participant participant = new Participant { Id = Guid.NewGuid(), UserId = _participant.UserId, MarathonDateId = _participant.MarathonDateId };
					_context.Participants.Add(participant);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<ActionResult> ClearSelfEstimationCheckList()
		{
			using (var _context = ApplicationDbContext.Create())
			{
				_context.Achievements.RemoveRange(await _context.Achievements.ToListAsync());
				_context.Resumes.RemoveRange(await _context.Resumes.ToListAsync());
				_context.Feedbacks.RemoveRange(await _context.Feedbacks.ToListAsync());
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}
	}
}