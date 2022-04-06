using Bongo.Core.Services;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Core.Tests
{
    [TestFixture]
    public class StudyRoomBookingServiceTests
    {
        private StudyRoomBooking _request;
        private List<StudyRoom> _availableRooms;
        private Mock<IStudyRoomBookingRepository> _studyRoomBookingRepoMock;
        private Mock<IStudyRoomRepository> _studyRoomRepoMock;
        private StudyRoomBookingService _studyRoomBookingService;

        [SetUp]
        public void InitialSetup()
        {
            _request = new StudyRoomBooking
            {
                FirstName = "Ben",
                LastName = "Spark",
                Email = "ben@gmail.com",
                Date = new DateTime(2022, 1, 1)
            };
            _availableRooms = new List<StudyRoom>
            {
                new StudyRoom
                {
                    Id = 10, RoomName = "Michigan", RoomNumber = "A202"
                }
            };
            _studyRoomBookingRepoMock = new Mock<IStudyRoomBookingRepository>();
            _studyRoomRepoMock = new Mock<IStudyRoomRepository>();
            _studyRoomRepoMock.Setup(s => s.GetAll()).Returns(_availableRooms);
            _studyRoomBookingService = new StudyRoomBookingService(_studyRoomBookingRepoMock.Object, _studyRoomRepoMock.Object);
        }

        [Test]
        public void GetAllBooking_CallMethod_VerifyRepoGetAllMethodCalledOnce()
        {
            _studyRoomBookingService.GetAllBooking();
            _studyRoomBookingRepoMock.Verify(s => s.GetAll(null), Times.Once);
        }

        [Test]
        public void BookStudyRoom_InvokeMethodAndPassNullRequest_ThrowArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _studyRoomBookingService.BookStudyRoom(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'request')", exception.Message);
            Assert.AreEqual("request", exception.ParamName);
        }

        [Test]
        public void BookStudyRoom_SaveRequestWithAvailableRoom_ReturnsResultWithAllValues()
        {
            StudyRoomBooking savedBooking = null;
            _studyRoomBookingRepoMock.Setup(s => s.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking => savedBooking = booking);

            _studyRoomBookingService.BookStudyRoom(_request);

            _studyRoomBookingRepoMock.Verify(s => s.Book(It.IsAny<StudyRoomBooking>()), Times.Once);
            Assert.AreEqual(_request.FirstName, savedBooking.FirstName);
            Assert.AreEqual(_request.LastName, savedBooking.LastName);
            Assert.AreEqual(_request.Email, savedBooking.Email);
            Assert.AreEqual(_request.Date, savedBooking.Date);
            Assert.AreEqual(_availableRooms.First().Id, savedBooking.StudyRoomId);
        }

        [Test]
        public void BookStudyRoom_InputRequest_ResultPropertiesMatchRequestProperties()
        {
            StudyRoomBookingResult result = _studyRoomBookingService.BookStudyRoom(_request);

            Assert.IsNotNull(result);
            Assert.AreEqual(_request.FirstName, result.FirstName);
            Assert.AreEqual(_request.LastName, result.LastName);
            Assert.AreEqual(_request.Email, result.Email);
            Assert.AreEqual(_request.Date, result.Date);
        }

        [TestCase(true, ExpectedResult = StudyRoomBookingCode.Success)]
        [TestCase(false, ExpectedResult = StudyRoomBookingCode.NoRoomAvailable)]
        public StudyRoomBookingCode BookStudyRoom_FirstCaseRoomAvailableSecondNot_ReturnsFirstSuccesSocondFailure(bool isRoomAvailable)
        {
            if (!isRoomAvailable)
                _availableRooms.Clear();

            return _studyRoomBookingService.BookStudyRoom(_request).Code;
        }

        [TestCase(0, false)]
        [TestCase(55, true)]
        public void BookStudyRoom_BookRoomAndAvilabilityCanChange_ReturnsBookingId(int bookingId, bool isAvailable)
        {
            if (!isAvailable)
                _availableRooms.Clear();

            _studyRoomBookingRepoMock.Setup(s => s.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking => booking.BookingId = 55 );

            var result = _studyRoomBookingService.BookStudyRoom(_request);

            Assert.AreEqual(bookingId, result.BookingId);
            if (!isAvailable)
                _studyRoomBookingRepoMock.Verify(s => s.Book(It.IsAny<StudyRoomBooking>()), Times.Never);
        }
    }
}
