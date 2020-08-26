using Countdown.Models;
using Countdown.Models.Data;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace Countdown.Controllers
{
    [Authorize]
    public class TodoController : AccountController
    {
        private ITodoRepository todoRepository;
        public TodoController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ITodoRepository todoRepository) : base(userManager, signInManager)
        {
            this.todoRepository = todoRepository;
        }



        // GET: ToDo

        public ActionResult Create()
        {
            CreateTodoViewModel model = CreateTodoViewModel();


            return View(model);
        }

        private ApplicationUser GetCurrentUser()
        {
            ApplicationUser user = UserManager.Users.First(x => x.UserName.Equals(User.Identity.Name));
            return user;

        }
        private CreateTodoViewModel CreateTodoViewModel()
        {

            List<SelectListItem> registeredUsers = GetRegisteredUsers();

            CreateTodoViewModel model = new CreateTodoViewModel
            {
                StartDate = DateTime.Now.Date.ToString("yyyy/MM/dd"),
                StartTime = DateTime.Now.ToString("hh:mm:ss tt"),
                RegisteredUsers = registeredUsers,
                Owner = GetCurrentUser().Id
            };

            return model;
        }

        private List<SelectListItem> GetRegisteredUsers()
        {
            ApplicationUser user = GetCurrentUser();

            List<SelectListItem> registeredUsers = UserManager.Users.Where(x => x.UserName != user.UserName).OrderBy(x => x.Email).Select(x => new SelectListItem { Text = x.LastName + ", " + x.FirstName + " " + x.Email, Value = x.Id }).ToList();

            registeredUsers.Insert(0, new SelectListItem { Text = user.LastName + ", " + user.FirstName + " " + user.Email, Value = user.Id });
            return registeredUsers;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTodoViewModel model)
        {

            if (ModelState.IsValid)
            {
                DateTime startDate = GetDateTimeFromString(model.StartDate, model.StartTime);
                DateTime dueDate = GetDateTimeFromString(model.DueDate, model.DueTime);

                Todo todo = new Todo
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    DueDate = dueDate,
                    AssignedTo = model.AssignedTo,
                    Owner = model.Owner
                };

                todoRepository.Create(todo);
                TempData["TodoSuccess"] = "Todo item " + todo.Title + " added successfully";
                return RedirectToAction("Index", "Home");

            }

            model.RegisteredUsers = GetRegisteredUsers();
            return View(model);





        }

        private DateTime GetDateTimeFromString(String date, String time)
        {
            DateTime.TryParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime startDate);

            DateTime.TryParseExact(time, "hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime startTime);

            startDate = startDate.Date + new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second);
            return startDate;
        }

        public ActionResult List()
        {
            ApplicationUser user = GetCurrentUser();
            IEnumerable<TodoViewModel> todoViewModels = todoRepository.Todoes.Select(x => new TodoViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                AssignedToFirstName = GetUser(x.AssignedTo).FirstName,
                CurrentUserId = user.Id,
                IsCompleted = x.DueDate <= DateTime.Now,
                Owner = x.Owner,
                OwnerFirstName = GetUser(x.Owner).FirstName,
                TimeLeft = x.DueDate.Subtract(DateTime.Now).TotalMilliseconds


            });

            return View(new ViewTodoViewModel
            {
                TodoFilter = new TodoFilter(),
                TodoViewModels = todoViewModels

            });
        }

        private ApplicationUser GetUser(string id)
        {
            ApplicationUser user = UserManager.Users.FirstOrDefault(x => x.Id.Equals(id));
            return user;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter(ViewTodoViewModel model)
        {
            ApplicationUser user = GetCurrentUser();
            IEnumerable<TodoViewModel> todoViewModels = todoRepository.Todoes.Select(x => new TodoViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                AssignedToFirstName = GetUser(x.AssignedTo).FirstName,
                CurrentUserId = user.Id,
                IsCompleted = x.DueDate <= DateTime.Now,
                Owner = x.Owner,
                OwnerFirstName = GetUser(x.Owner).FirstName,
                TimeLeft = x.DueDate.Subtract(DateTime.Now).TotalMilliseconds


            });

            if (model.TodoFilter.OwnedByMe)
            {
                todoViewModels = todoViewModels.Where(p => p.Owner.Equals(user.Id));
            }

            if (model.TodoFilter.OwnedByOthers)
            {
                todoViewModels = todoViewModels.Where(p => !p.Owner.Equals(user.Id));
            }

            if (model.TodoFilter.AssignedToOthers)
            {
                todoViewModels = todoViewModels.Where(p => !p.AssignedTo.Equals(user.Id));
            }

            if (model.TodoFilter.Completed)
            {
                todoViewModels = todoViewModels.Where(p => p.TimeLeft <= 0);
            }

            if (model.TodoFilter.Pending)
            {
                todoViewModels = todoViewModels.Where(p => p.TimeLeft > 0);
            }

            model.TodoViewModels = todoViewModels;
            return View("List", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkComplete(int id)
        {
            Todo item = todoRepository.MarkComplete(id);
           
            if(item == null)
            {
                ViewBag.ErrorMessage = "Todo Item could not be marked as complete";
            }
            else
            {
                ViewBag.SuccessMessage = "Todo Item marked as complete successfully";
            }
            ApplicationUser user = GetCurrentUser();
            IEnumerable<TodoViewModel> todoViewModels = todoRepository.Todoes.Select(x => new TodoViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                AssignedToFirstName = GetUser(x.AssignedTo).FirstName,
                CurrentUserId = user.Id,
                IsCompleted = x.DueDate <= DateTime.Now,
                Owner = x.Owner,
                OwnerFirstName = GetUser(x.Owner).FirstName,
                TimeLeft = x.DueDate.Subtract(DateTime.Now).TotalMilliseconds


            });

            return View("List",new ViewTodoViewModel
            {
                TodoFilter = new TodoFilter(),
                TodoViewModels = todoViewModels

            });
        }
    }
}