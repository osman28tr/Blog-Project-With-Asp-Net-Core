﻿using BusinessLayer.Concrete;
using DataAccessLayer.EntityFrameworkCore;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core_Proje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestimonialController : Controller
    {
        TestimonialManager testimonialManager = new TestimonialManager(new EfTestimonialDal());
        public IActionResult Index()
        {
            var values = testimonialManager.TGetList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddTestimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTestimonial(Testimonial testimonial)
        {
            testimonialManager.TAdd(testimonial);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteTestimonial(int id)
		{
            var values = testimonialManager.TGetById(id);
            testimonialManager.TDelete(values);
            return RedirectToAction("Index");
		}
        [HttpGet]
        public IActionResult EditTestimonial(int id)
        {
            var testimonialValue = testimonialManager.TGetById(id);
            return View(testimonialValue);
        }
        [HttpPost]
        public IActionResult EditTestimonial(Testimonial testimonial)
        {
            testimonialManager.TUpdate(testimonial);
            return RedirectToAction("Index");
        }

    }
}
