using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using dymj.ReproductorMusica.API_SQL.Model;
using dymj.ReproductorMusica.API_SQL.Data;

namespace dymj.ReproductorMusica.API_SQL.Services;

public class LlistaServicce
{
    private readonly DataContext _context;
    public LlistaService(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Accedeix a la ruta /api/Llista/getLlistes per obtenir totes les llistes
    /// </summary>
    /// <returns>El llistat de Llistes de reproduccio</returns>
    public async Task<List<Llista>> GetAsync() {
        return await _context.Llistes.ToListAsync();
    }

    /// <summary>   
    /// Accedeix a la ruta /api/Llista/getLlista/{MACAddress}/{NomLlista} per obtenir una Llista de reproduccio
    /// </summary>
    /// <param name="MACAddress">MACAddress de la Llista de reproduccio a obtenir</param>
    /// <param name="NomLlista">Nom de la Llista de reproduccio a obtenir</param>
    /// <returns>L'objecte de la Llista de reproduccio</returns>
    public async Task<Llista?> GetAsync(string MACAddress, string NomLlista) =>
        await _context.Llistes
                            .Include(x => x.LCancons)
                            .FirstOrDefaultAsync(x => x.MACAddress == MACAddress && x.NomLlista == NomLlista);ç

    /// <summary>
    /// Accedeix a la ruta /api/Llista/postLlista per crear una Llista de reproduccio
    /// </summary>
    /// <param name="newLlista">L'objecte de la Llista de reproduccio a crear</param>
    /// <returns>Verificacio de que la Llista de reproduccio s'ha creat correctament</returns>
    public async Task CreateAsync(Llista newLlista) {
        await _context.Llistes.AddAsync(newLlista);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Accedeix a la ruta /api/Llista/putLlista/{MACAddress}/{NomLlista} per modificar una Llista de reproduccio
    /// </summary>
    /// <param name="MACAddress">MACAddress de la Llista de reproduccio a modificar</param>
    /// <param name="NomLlista">Nom de la Llista de reproduccio a modificar</param>
    /// <param name="updatedLlista">L'objecte de la Llista de reproduccio a modificar</param>
    /// <returns>Verificacio de que la Llista de reproduccio s'ha modificat correctament</returns>
    public async Task UpdateAsync(string MACAddress, string NomLlista, Llista updatedLlista) {
        var llista = await _context.Llistes
                            .FirstOrDefaultAsync(x => x.MACAddress == MACAddress && x.NomLlista == NomLlista);
        
        if (MACAddress == updatedLlista.MACAddress && NomLlista == updatedLlista.NomLlista && llista != null)
        {
            _context.Entry(updatedLlista).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } else {
            return NotFound();
        }
    }

    /// <summary>
    /// Accedeix a la ruta /api/Llista/deleteLlista/{MACAddress}/{NomLlista} per eliminar una Llista de reproduccio
    /// </summary>
    /// <param name="MACAddress">MACAddress de la Llista de reproduccio a eliminar</param>
    /// <param name="NomLlista">Nom de la Llista de reproduccio a eliminar</param>
    /// <returns>Verificacio de que la Llista de reproduccio s'ha eliminat correctament</returns>
    public async Task RemoveAsync(string MACAddress, string NomLlista) {
        var llista = await _context.Llistes
                            .FirstOrDefaultAsync(x => x.MACAddress == MACAddress && x.NomLlista == NomLlista);

        if (llista != null) {
            _context.Llistes.Remove(llista);
            await _context.SaveChangesAsync();
        }
    }
}
