using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialMIgration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNo = table.Column<long>(type: "bigint", nullable: true),
                    AltNo = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsLogged = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Person__AA2FFB8585A481E5", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    TimeSlotID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TimeSlot__41CC1F523BDF29CF", x => x.TimeSlotID);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    Speciality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    YearsOfReg = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Doctor__AA2FFB85B73EBE0D", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK__Doctor__PersonID__403A8C7D",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                });

            migrationBuilder.CreateTable(
                name: "Treatment_Done",
                columns: table => new
                {
                    TId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Prescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    FollowUp = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Treatmen__C456D749F8D54564", x => x.TId);
                    table.ForeignKey(
                        name: "FK__Treatment__Docto__6383C8BA",
                        column: x => x.DoctorID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__Treatment__Patie__6477ECF3",
                        column: x => x.PatientID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeSlotID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__8ECDFCA2C32C1BA5", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK__Appointme__Docto__4D94879B",
                        column: x => x.DoctorID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__Appointme__Patie__4E88ABD4",
                        column: x => x.PatientID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__Appointme__TimeS__4F7CD00D",
                        column: x => x.TimeSlotID,
                        principalTable: "TimeSlot",
                        principalColumn: "TimeSlotID");
                });

            migrationBuilder.CreateTable(
                name: "DoctorTimeSlotMapping",
                columns: table => new
                {
                    MappingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    TimeSlotID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DoctorTi__8B5781BDA9090B16", x => x.MappingID);
                    table.ForeignKey(
                        name: "FK__DoctorTim__Docto__797309D9",
                        column: x => x.DoctorID,
                        principalTable: "Doctor",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__DoctorTim__TimeS__7A672E12",
                        column: x => x.TimeSlotID,
                        principalTable: "TimeSlot",
                        principalColumn: "TimeSlotID");
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistory",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TId = table.Column<int>(type: "int", nullable: false),
                    Records = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalH__4D7B4ADDAADCB26E", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK__MedicalHi__Patie__7D439ABD",
                        column: x => x.PatientID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__MedicalHist__TId__7E37BEF6",
                        column: x => x.TId,
                        principalTable: "Treatment_Done",
                        principalColumn: "TId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentID = table.Column<int>(type: "int", nullable: true),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E32C5C3E4B7", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK__Notificat__Appoi__5441852A",
                        column: x => x.AppointmentID,
                        principalTable: "Appointment",
                        principalColumn: "AppointmentID");
                    table.ForeignKey(
                        name: "FK__Notificat__Docto__52593CB8",
                        column: x => x.DoctorID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                    table.ForeignKey(
                        name: "FK__Notificat__Patie__534D60F1",
                        column: x => x.PatientID,
                        principalTable: "Person",
                        principalColumn: "PersonID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorID",
                table: "Appointment",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientID",
                table: "Appointment",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_TimeSlotID",
                table: "Appointment",
                column: "TimeSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeSlotMapping_DoctorID",
                table: "DoctorTimeSlotMapping",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeSlotMapping_TimeSlotID",
                table: "DoctorTimeSlotMapping",
                column: "TimeSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientID",
                table: "MedicalHistory",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_TId",
                table: "MedicalHistory",
                column: "TId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppointmentID",
                table: "Notifications",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DoctorID",
                table: "Notifications",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PatientID",
                table: "Notifications",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_Done_DoctorID",
                table: "Treatment_Done",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_Done_PatientID",
                table: "Treatment_Done",
                column: "PatientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorTimeSlotMapping");

            migrationBuilder.DropTable(
                name: "MedicalHistory");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Treatment_Done");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "TimeSlot");
        }
    }
}
