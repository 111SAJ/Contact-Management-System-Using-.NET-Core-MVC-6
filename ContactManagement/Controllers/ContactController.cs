using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        //List Contact
        [HttpGet]
        public IActionResult Index(string searchEmail)
        {
            var contactList = _context.Contact.AsQueryable();

            if (!string.IsNullOrEmpty(searchEmail))
            {
                contactList = contactList.Where(c => c.Email.Contains(searchEmail));
            }

            return View(contactList.ToList());
        }

        //Register Contact
        [HttpGet]
        public IActionResult Create()
        {
            //var newContact = new Contact();
            //return PartialView("_ContactModalPartial", newContact);
            Contact cont = new Contact();
            return PartialView("_ContactModalPartial", cont);
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contact.Add(contact);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    contact = new
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        isFavorite = contact.isFavorite
                    }
                });
            }

            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }


        // Edit Contact
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contact = _context.Contact.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            return PartialView("_ContactModalPartial", contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the contact in the database
                    _context.Contact.Update(contact);
                    _context.SaveChanges();

                    // Return the updated contact to refresh the UI
                    return Json(new { success = true, contact = contact });
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    ModelState.AddModelError(string.Empty, "Error updating contact. Please try again.");
                }
            }

            // Return the partial view with validation errors
            return PartialView("_ContactModalPartial", contact);
        }




        // Delete Contact
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var contact = _context.Contact.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contact.Remove(contact);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // Toggle favorite status
        [HttpPost]
        public IActionResult ToggleFavorite(int contactId, bool isFavorite)
        {
            var contact = _context.Contact.Find(contactId);
            if (contact == null)
            {
                return NotFound();
            }

            contact.isFavorite = isFavorite;
            _context.SaveChanges();

            // Return updated contact with success status
            return Json(new { success = true, contact });
        }







    }
}
