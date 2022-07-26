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

		ApplicationDbContext _context = ApplicationDbContext.Create();

		[Authorize(Roles = "admin")]
		public ActionResult Index()
		{
			if (ModelState.IsValid)
			{
				var users = _context.Users.ToList();
				return View(users);
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
				marathon.Id = Guid.NewGuid();
				_context.Marathons.Add(marathon);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public ActionResult SetMarathonDate()
		{
			ViewBag.Marathons = new SelectList(_context.Marathons.ToList(), "Id", "Name");
			return PartialView();
		}
		//public async Task<ActionResult> SetMarathonDate()
		//{
		//	ViewBag.Marathons = new SelectList(await _context.Marathons.ToListAsync(), "Id", "Name");
		//	return PartialView();
		//}

		[HttpPost]
		public async Task<ActionResult> SetMarathonDate(MarathonDate _marathonDate)
		{
			if(ModelState.IsValid)
			{
				var id = Guid.NewGuid();
				DateTime date = new DateTime(_marathonDate.Year, _marathonDate.Month, _marathonDate.Day);
				MarathonDate marathonDate = new MarathonDate { Id = id, MarathonId = _marathonDate.MarathonId, StartDate = date };
				_context.MarathonDates.Add(marathonDate);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "admin")]
		public ActionResult AddParticipants()
		{
			ViewBag.MarathonDates = new SelectList(_context.MarathonDates.ToList(), "Id", "StartDate");
			ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "UserName");
			return PartialView();
		}

		[HttpPost]
		public async Task<ActionResult> AddParticipants(Participant _participant)
		{
			if(ModelState.IsValid)
			{
				Participant participant = new Participant { Id = Guid.NewGuid(), UserId = _participant.UserId, MarathonDateId = _participant.MarathonDateId };
				_context.Participants.Add(participant);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult ClearSelfEstimationCheckList()
		{
			_context.Achievements.RemoveRange(_context.Achievements.ToList());
			_context.Resumes.RemoveRange(_context.Resumes.ToList());
			_context.Feedbacks.RemoveRange(_context.Feedbacks.ToList());
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}