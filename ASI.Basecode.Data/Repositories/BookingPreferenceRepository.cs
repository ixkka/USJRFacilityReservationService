using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ASI.Basecode.Data.Repositories/BookingPreferenceRepository.csx
// ASI.Basecode.Data/BookingPreferenceRepository.cs
namespace ASI.Basecode.Data.Repositories
{
    public class BookingPreferenceRepository : IBookingPreferenceRepository
    {
        private readonly AsiBasecodeDBContext _context;

        public BookingPreferenceRepository(AsiBasecodeDBContext context)
        {
            _context = context;
        }

        // Implementing AddPreference method
        public void AddPreference(BookingPreference bookingPreference)
        {
            _context.BookingPreferences.Add(bookingPreference);
            _context.SaveChanges();
        }

        // Implementing UpdatePreference method
        public void UpdatePreference(BookingPreference bookingPreference)
        {
            _context.BookingPreferences.Update(bookingPreference);
            _context.SaveChanges();
        }
    }
}


