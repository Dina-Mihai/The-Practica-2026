using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipant { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConversationParticipant>()
                 .HasKey(cp => new { cp.ConversationID, cp.UserID });

            // message many to user one (sender)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Message)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            // user one to ConversationParticipant many
            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.ConversationParticipants)
                .HasForeignKey(cp => cp.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // ConversationParticipant one to Conversation one
            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.Conversation)
                .WithMany(c => c.ConversationParticipants)
                .HasForeignKey(cp => cp.ConversationID)
                .OnDelete(DeleteBehavior.Restrict);

            // message many to conversation one
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationID)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate participant rows
            modelBuilder.Entity<ConversationParticipant>()
                .HasIndex(cp => new { cp.ConversationID, cp.UserID })
                .IsUnique();

        }
        //dai update la migrare


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

       


    }
}
