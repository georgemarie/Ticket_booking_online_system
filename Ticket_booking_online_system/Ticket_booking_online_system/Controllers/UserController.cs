using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    /// <summary>
    /// //////////////////////////////////////NOURRRR
    /// </summary>
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        // GET: UserController
        public UserController(IGenericRepository<User> userRepository) 
        { 
            _userRepository = userRepository;
        }

        #region  Admin Authorization
        //   [Authorize(Roles = "Admin")]
        // GET: /User
        [HttpGet("")]
        public ActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }



        // [Authorize(Roles = "Admin")]
        // GET: /User/Delete/5
        [HttpGet("Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View();
        }


     
        // [Authorize(Roles = "Admin")]
        // POST: /User/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetById(model.UserID);
                if (user == null)
                {
                    return NotFound();
                }
                _userRepository.Delete(user);
                _userRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        #endregion

        [HttpGet("GetUserProfile")]
        // GET: /User/GetUserProfile
        public ActionResult Details(int id)
        {
            var user = _userRepository.GetById(id);
            if(user == null) {  return NotFound(); }
            return View(user);
        }

        // GET: /User/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: /User/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                model.Created_at = DateTime.Now;
                _userRepository.Add(model);
                _userRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: /User/Edit/5
        [HttpGet("Edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            var user = _userRepository.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // POST: /User/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost("Edit/{id:int}")]
        public ActionResult Edit( User model)
        {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = _userRepository.GetById(model.UserID);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Passport_num = model.Passport_num;
                  //user.Role = model.Role;      

               _userRepository.Update(user);
                _userRepository.Save();

                return RedirectToAction(nameof(Index));
            }
        }        
 }
