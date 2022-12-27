using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;

namespace Ferdinand.Data.EntityFrameworkCore.Repositories;

public sealed class ColorRepository : IColorRepository
{
    public Task<int> Delete(Guid key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(Guid key)
    {
        throw new NotImplementedException();
    }

    public Task<Color> Get(Guid key)
    {
        throw new NotImplementedException();
    }

    public Task<Color> Save(Color aggregate)
    {
        throw new NotImplementedException();
    }
}