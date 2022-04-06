using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.DataAccess.Tests
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking studyRoomBooking_One;
        private StudyRoomBooking studyRoomBooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        public StudyRoomBookingRepositoryTests()
        {
            studyRoomBooking_One = new StudyRoomBooking
            {
                FirstName = "Ben1",
                LastName = "Spark1",
                Date = new DateTime(2023, 1, 1),
                Email = "ben1@gmail.com",
                BookingId = 11,
                StudyRoomId = 1
            };
            studyRoomBooking_Two = new StudyRoomBooking
            {
                FirstName = "Ben2",
                LastName = "Spark2",
                Date = new DateTime(2023, 2, 2),
                Email = "ben2@gmail.com",
                BookingId = 22,
                StudyRoomId = 2
            };
        }

        [SetUp]
        public void InitialSetup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_db").Options;
        }

        [Test]
        [Order(1)]
        public void Book_SaveStudyRoomBookingOneToDB_CompareSavedItemWithBookingOne()
        {
            //Arrange

            //Act
            using(var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);

                repository.Book(studyRoomBooking_One);
            }

            //Assert
            using(var context = new ApplicationDbContext(options))
            {
                var bookingFromDB = context.StudyRoomBookings.FirstOrDefault(s => s.BookingId == studyRoomBooking_One.BookingId);

                Assert.AreEqual(studyRoomBooking_One.BookingId, bookingFromDB.BookingId);
                Assert.AreEqual(studyRoomBooking_One.FirstName, bookingFromDB.FirstName);
                Assert.AreEqual(studyRoomBooking_One.LastName, bookingFromDB.LastName);
                Assert.AreEqual(studyRoomBooking_One.Date, bookingFromDB.Date);
                Assert.AreEqual(studyRoomBooking_One.Email, bookingFromDB.Email);
            }
        }

        [Test]
        [Order(2)]
        public void GetAll_SaveBothBookingItems_CompareSavedEntitiesWithBookingItems()
        {
            //Arrange
            var expectedList = new List<StudyRoomBooking> { studyRoomBooking_One, studyRoomBooking_Two };

            using(var context = new ApplicationDbContext(options))
            { 
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
                repository.Book(studyRoomBooking_Two);
            }

            //Act
            List<StudyRoomBooking> savedList;
            using(var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                savedList = repository.GetAll(null).ToList();
            }

            //Assert
            CollectionAssert.AreEqual(expectedList, savedList, new BookingComparer());
        }

        private class BookingComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var booking1 = (StudyRoomBooking) x;
                var booking2 = (StudyRoomBooking) y;

                if (booking1.BookingId != booking2.BookingId)
                    return 1;

                else if (booking1.FirstName != booking2.FirstName)
                    return 1;

                else if (booking1.LastName != booking2.LastName)
                    return 1;

                else if (booking1.Date != booking2.Date)
                    return 1;

                else if (booking1.Email != booking2.Email)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
