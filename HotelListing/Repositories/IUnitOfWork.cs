using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Hotel> HotelGenericRepository { get; }
        IGenericRepository<Country> CountryGenericRepository { get; }
        Task Save();
    }
}
