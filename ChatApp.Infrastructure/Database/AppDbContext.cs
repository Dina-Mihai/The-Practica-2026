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
            //mesaje many to user one
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>()
                    .HasOne(m => m.Sender)
                    .WithMany(u => u.Message)
                    .HasForeignKey(m => m.SenderID)
                    .OnDelete(DeleteBehavior.Restrict);

            // user one to Coversation Participant many
            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(u => u.User)
                .WithMany(m => m.ConversationParticipants)
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            //Coversation Participant one  to Conversaton one
            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(u => u.Conversation)
                .WithMany(m => m.ConversationParticipants)
                .HasForeignKey(m => m.ConversationID)//de ce am nevoie de <ConversationParticipant>?
                .OnDelete(DeleteBehavior.Restrict);

            //message many to conversation one
            modelBuilder.Entity<Message>()
                    .HasOne(m => m.Conversation)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(m => m.ConversationID)
                    .OnDelete(DeleteBehavior.Cascade);
            // Prevent duplicate participant rows
            modelBuilder.Entity<ConversationParticipant>()
                .HasIndex(cp => new { cp.ConversationID, cp.User })
                .IsUnique();
        }
        //dai update la migrare


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

       


    }
}
