using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Hotel> _hotels;
        private IGenericRepository<Country> _countries;
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Hotel> HotelGenericRepository => _hotels ??=
            new GenericRepository<Hotel>(_context);

        public IGenericRepository<Country> CountryGenericRepository => _countries ??=
            new GenericRepository<Country>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
