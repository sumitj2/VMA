using Database.Abstraction.VMA.Contract;
using Database.VMA.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Database.VMA.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly VendorManagementDbContext _context;

        public VendorRepository(VendorManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddVendors(Vendor VendorEntity)
        {
            await _context.AddAsync(VendorEntity).ConfigureAwait(true);
            await _context.SaveChangesAsync();
        }
        public async Task EditUpdateVendors(Vendor VendorEntity)
        {
            /// var result = await GetVendorsById(VendorEntity.VendorId);
            if (VendorEntity != null)
            {
                _context.Vendors.Update(VendorEntity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Vendor>> GetAllVendors()
        {
            return await _context.Vendors.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Vendor?> GetVendorsById(int vendorId)
        {
            return await _context.Vendors.Where(x => x.IsActive == true && x.VendorId == vendorId).FirstOrDefaultAsync();
        }

        public async Task RemoveVendor(Vendor VendorEntity)
        {
            _context.Vendors.Remove(VendorEntity);
            await _context.SaveChangesAsync();
        }
        public int SaveImportedVendorsToDatabase(DataTable dataTable)
        {
            //using (var context = new VendorManagementDbContext())
            //{
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entities = new List<Vendor>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var entity = new Vendor
                        {
                            //Column1 = row["Column1"].ToString(),
                            //Column2 = row["Column2"].ToString()
                            // Add more mappings as needed
                        };
                        entities.Add(entity);
                    }

                    _context.Vendors.AddRange(entities);
                    _context.SaveChanges();

                    // Commit the transaction if everything is successful
                    transaction.Commit();

                    return 1;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    // Optionally, log the exception or rethrow it
                    Console.WriteLine($"An error occurred: {ex.Message}");

                    return 0;
                }
            }
            // }
        }

    }
}
