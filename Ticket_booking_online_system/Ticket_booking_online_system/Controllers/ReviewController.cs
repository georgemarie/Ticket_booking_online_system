using BLL.Repository.Interfaces;
using DAL.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Review")]
    //[Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        // GET: Reviews
        //[AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index()
        {
            var reviews = _reviewRepository.GetAll();
            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetById(id);
            if (review == null) return NotFound();
            return View(review);
        }

        #region Admin Controller
        // GET: Reviews/Create
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
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
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(review);
            }
        }

        // GET: ReviewController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetById(id);
            if (review == null) return NotFound();
            return View(review);
        }

        // POST: ReviewController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.Update(review);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(review);
            }
        }

        // GET: ReviewController/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var review = _reviewRepository.GetById(id);
            if (review == null) return NotFound();
            return View(review);
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.Delete(review);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        } 
        #endregion
    }
}
