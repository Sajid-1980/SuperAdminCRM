using SuperAdmin.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using SuperAdmin.Models.Masterleads;
using SuperAdmin.Models;
namespace SuperAdmin.Areas.Identity.Data;

public class SuperAdminContext : IdentityDbContext<SuperAdminUser>
{
    public DbSet<SubmitOrderModel> SubmitOrderModel { get; set; }
    public DbSet<Notificationscs> Notificationscs { get; set; }
    public DbSet<Masterleads> Masterleads { get; set; }
    public DbSet<ServiceModel> Services { get; set; }
    public DbSet<Subscription> Subscription { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Register> Register { get; set; }
    public DbSet<MaskOrder> MaskOrder { get; set; }
    public DbSet<Balance> Balance { get; set; }
    public DbSet<SmsBalancecs> SmsBalancecs { get; set; }
    
    public DbSet<DetuctionType> DetuctionType { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<LeadRate> LeadRate { get; set; }
    public DbSet<Package> Package{ get; set; }
    public SuperAdminContext(DbContextOptions<SuperAdminContext> options)
        : base(options)
    {

    }

}