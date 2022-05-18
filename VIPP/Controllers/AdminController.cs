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
	}
}