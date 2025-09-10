using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.ScaffoldModels;


namespace Models.ScaffoldDbContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }

        public virtual DbSet<Doctor> Doctors { get; set; }

        public virtual DbSet<DoctorTimeSlotMapping> DoctorTimeSlotMappings { get; set; }

        public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<TimeSlot> TimeSlots { get; set; }

        public virtual DbSet<TreatmentDone> TreatmentDones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2C32C1BA5");

                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
                entity.Property(e => e.IsDeleted)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.PatientId).HasColumnName("PatientID");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TimeSlotId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TimeSlotID");

                entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentDoctors)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appointme__Docto__4D94879B");

                entity.HasOne(d => d.Patient).WithMany(p => p.AppointmentPatients)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appointme__Patie__4E88ABD4");

                entity.HasOne(d => d.TimeSlot).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK__Appointme__TimeS__4F7CD00D");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.PersonId).HasName("PK__Doctor__AA2FFB85B73EBE0D");

                entity.ToTable("Doctor");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PersonID");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Speciality)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Person).WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Doctor__PersonID__403A8C7D");
            });

            modelBuilder.Entity<DoctorTimeSlotMapping>(entity =>
            {
                entity.HasKey(e => e.MappingId).HasName("PK__DoctorTi__8B5781BDA9090B16");

                entity.ToTable("DoctorTimeSlotMapping");

                entity.Property(e => e.MappingId).HasColumnName("MappingID");
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
                entity.Property(e => e.TimeSlotId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TimeSlotID");

                entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorTimeSlotMappings)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DoctorTim__Docto__797309D9");

                entity.HasOne(d => d.TimeSlot).WithMany(p => p.DoctorTimeSlotMappings)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK__DoctorTim__TimeS__7A672E12");
            });

            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId).HasName("PK__MedicalH__4D7B4ADDAADCB26E");

                entity.ToTable("MedicalHistory");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Dtype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DType");
                entity.Property(e => e.PatientId).HasColumnName("PatientID");
                entity.Property(e => e.Tid).HasColumnName("TId");

                entity.HasOne(d => d.Patient).WithMany(p => p.MedicalHistories)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicalHi__Patie__7D439ABD");

                entity.HasOne(d => d.TidNavigation).WithMany(p => p.MedicalHistories)
                    .HasForeignKey(d => d.Tid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicalHist__TId__7E37BEF6");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32C5C3E4B7");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.PatientId).HasColumnName("PatientID");
                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Appointment).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.AppointmentId)
                    .HasConstraintName("FK__Notificat__Appoi__5441852A");

                entity.HasOne(d => d.Doctor).WithMany(p => p.NotificationDoctors)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Docto__52593CB8");

                entity.HasOne(d => d.Patient).WithMany(p => p.NotificationPatients)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Patie__534D60F1");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFB8585A481E5");

                entity.ToTable("Person");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Dob).HasColumnName("DOB");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.HasKey(e => e.TimeSlotId).HasName("PK__TimeSlot__41CC1F523BDF29CF");

                entity.ToTable("TimeSlot");

                entity.Property(e => e.TimeSlotId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TimeSlotID");
                entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            });

            modelBuilder.Entity<TreatmentDone>(entity =>
            {
                entity.HasKey(e => e.Tid).HasName("PK__Treatmen__C456D749F8D54564");

                entity.ToTable("Treatment_Done");

                entity.Property(e => e.Tid).HasColumnName("TId");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
                entity.Property(e => e.Dtype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DType");
                entity.Property(e => e.FollowUp).HasColumnType("datetime");
                entity.Property(e => e.PatientId).HasColumnName("PatientID");
                entity.Property(e => e.Prescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Doctor).WithMany(p => p.TreatmentDoneDoctors)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Treatment__Docto__6383C8BA");

                entity.HasOne(d => d.Patient).WithMany(p => p.TreatmentDonePatients)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Treatment__Patie__6477ECF3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
