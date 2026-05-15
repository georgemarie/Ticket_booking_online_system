using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Ticket_booking_online_system.Controllers
{
    [Authorize]
    [Route("Review")]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IServiceRepository _serviceRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReviewRepository reviewRepository,
                                IServiceRepository serviceRepo,
                                UserManager<ApplicationUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _serviceRepo = serviceRepo;
            _userManager = userManager;
        }

        [AllowAnonymous] // Anyone can view reviews
        [HttpGet("")]
        public ActionResult Index()
        {
            var reviews = _reviewRepository.GetAllWithIncludes();
            return View(reviews);
        }

        [AllowAnonymous]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [Authorize(Roles = "User,Admin")] // Users should write reviews
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType");

            // If User, pre-select them. If Admin, let them choose.
            ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "Email");

            return View(new Review());
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.Add(review);
                _reviewRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType", review.ServiceID);
            ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "Email", review.UserID);
            return View(review);
        }

        [Authorize(Roles = "Admin")] // Only Admins should edit reviews
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();

            ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType", review.ServiceID);
            ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "Email", review.UserID);

            return View(review);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Review review)
        {
            if (id != review.ReviewID) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType", review.ServiceID);
                ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "Email", review.UserID);
                return View(review);
            }

            _reviewRepository.Update(review);
            _reviewRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")] // Only Admins can delete
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0) return BadRequest();
            var review = _reviewRepository.GetById(id);
            if (review == null) return NotFound();

            _reviewRepository.Delete(review);
            _reviewRepository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}