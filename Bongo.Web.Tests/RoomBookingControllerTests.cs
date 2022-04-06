using Bongo.Core.Services.IServices;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Web.Tests
{
    [TestFixture]
    public class RoomBookingControllerTests
    {
        private Mock<IStudyRoomBookingService> _studyRoomBookingMock;
        private RoomBookingController _roomBookingController;

        [SetUp]
        public void InitialSetup()
        {
            _studyRoomBookingMock = new Mock<IStudyRoomBookingService>();
            _roomBookingController = new RoomBookingController(_studyRoomBookingMock.Object);
        }

        [Test]
        public void Index_InvokeMethod_VerifyRepoGetAllBookingCalledOnce()
        {
            _roomBookingController.Index();
            _studyRoomBookingMock.Verify(s => s.GetAllBooking(), Times.Once);
        }
        [Test]
        public void Book_ModelStateError_ReturnToBookView()
        {
            _roomBookingController.ModelState.AddModelError("test", "test");

            var result = _roomBookingController.Book(new StudyRoomBooking());
            var viewResult = result as ViewResult;

            Assert.AreEqual("Book", viewResult.ViewName);
        }

        [Test]
        public void Book_SetupStudyBookRoomToReturnNoRoomAvailable_CheckViewData()
        {
            _studyRoomBookingMock.Setup(s => s.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns(new StudyRoomBookingResult { Code = StudyRoomBookingCode.NoRoomAvailable });

            var result = _roomBookingController.Book(new StudyRoomBooking());
            var viewResult = result as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("No Study Room available for selected date", viewResult.ViewData["Error"]);
        }

        [Test]
        public void Book_SuccessfullyBooked_ReturnsCodeAndRedirect()
        {
            //arrange
            _studyRoomBookingMock.Setup(s => s.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns((StudyRoomBooking booking) => new StudyRoomBookingResult
                {
                    Code = StudyRoomBookingCode.Success,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Email = booking.Email,
                    Date = booking.Date,
                });

            //act
            var result = _roomBookingController.Book(new StudyRoomBooking
            {
                FirstName = "Simon",
                LastName = "Smith",
                Email = "simon@smith.com",
                Date = new DateTime(2022, 1, 1)
            });

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.AreEqual("Simon", actionResult.RouteValues["FirstName"]);
            Assert.AreEqual(StudyRoomBookingCode.Success, actionResult.RouteValues["Code"]);
        }
    }
}
