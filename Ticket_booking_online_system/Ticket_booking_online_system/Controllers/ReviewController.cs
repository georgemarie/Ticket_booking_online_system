using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Review")]
    //[Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IServiceRepository _serviceRepo;
        private readonly IGenericRepository<User> _userRepository;

        public ReviewController(IReviewRepository reviewRepository,
                                IServiceRepository serviceRepo,
                                IGenericRepository<User> userRepository)
        {
            _reviewRepository = reviewRepository;
            _serviceRepo = serviceRepo;
            _userRepository = userRepository;
        }

        // GET: Reviews
        //[AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index()
        {
            var reviews = _reviewRepository.GetAllWithIncludes();
            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();
            return View(review);
        }

        // GET: Reviews/Create
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType");
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "UserID", "Email");

            return View(new Review());
        }
        // POST: Reviews/Create
        //[Authorize(Roles = "Admin")]
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
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "UserID", "Email", review.UserID);
            return View(review);
        
        }

        // GET: ReviewController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();

            ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType", review.ServiceID);
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "UserID", "Email", review.UserID);

            return View(review);
        }

        // POST: ReviewController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Review review)
        {
            if (id != review.ReviewID) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_serviceRepo.GetAll(), "ServiceID", "ServiceType", review.ServiceID);
                ViewBag.Users = new SelectList(_userRepository.GetAll(), "UserID", "Email", review.UserID);
                return View(review);
            }

            _reviewRepository.Update(review);
            _reviewRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: ReviewController/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetByIdWithIncludes(id);
            if (review == null) return NotFound();
            return View(review);
        }

        // POST: ReviewController/Delete/5
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
