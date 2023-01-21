using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using TestApplication.Models;

namespace TestApplication.DAL
{
    // Meeting
    public class MeetingEntityTypeConfiguration : IEntityTypeConfiguration<Meeting>
    {
        void IEntityTypeConfiguration<Meeting>.Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder
                .HasOne(c => c.AttachmentLibrary)
                .WithOne(c => c.Meeting)
                .HasForeignKey<AttachmentLibrary>(c => c.MeetingId);
                
        }

        
    }

    // Attachment
    public class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<Attachment>
    {
        void IEntityTypeConfiguration<Attachment>.Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder
               .HasOne(c => c.AttachmentLibrary)
               .WithMany(c => c.Attachments)
               .HasForeignKey(c => c.AttachmentLibraryID);
        }
    }

    // AttachmentLibrary
    public class AttachmentLibraryEntityTypeConfiguration : IEntityTypeConfiguration<AttachmentLibrary>
    {
        void IEntityTypeConfiguration<AttachmentLibrary>.Configure(EntityTypeBuilder<AttachmentLibrary> builder)
        {
            throw new NotImplementedException();
        }
    }

    

}
