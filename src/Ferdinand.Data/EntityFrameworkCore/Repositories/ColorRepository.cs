using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;

namespace Ferdinand.Data.EntityFrameworkCore.Repositories;

public sealed class ColorRepository : IColorRepository
{
    public Task<int> Delete(ColorId key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(ColorId key)
    {
        throw new NotImplementedException();
    }

    public Task<Color> Get(ColorId key)
    {
        throw new NotImplementedException();
    }

    public Task<Color> Save(Color aggregate)
    {
        throw new NotImplementedException();
    }
}