using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adventour.Api.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attraction_Info_Type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction_Info_Type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    continent_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    svg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    oauth_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    verified = table.Column<bool>(type: "bit", nullable: false),
                    photo_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_country = table.Column<int>(type: "int", nullable: false),
                    average_rating = table.Column<double>(type: "float", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    address_one = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_two = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    long_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attraction_Country_id_country",
                        column: x => x.id_country,
                        principalTable: "Country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itinerary",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_user = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itinerary", x => x.id);
                    table.ForeignKey(
                        name: "FK_Itinerary_Person_id_user",
                        column: x => x.id_user,
                        principalTable: "Person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attraction_Images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    picture_ref = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_attraction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Attraction_Images_Attraction_id_attraction",
                        column: x => x.id_attraction,
                        principalTable: "Attraction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attraction_Info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_attraction = table.Column<int>(type: "int", nullable: false),
                    id_attraction_info_type = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction_Info", x => x.id);
                    table.ForeignKey(
                        name: "FK_Attraction_Info_Attraction_Info_Type_id_attraction_info_type",
                        column: x => x.id_attraction_info_type,
                        principalTable: "Attraction_Info_Type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attraction_Info_Attraction_id_attraction",
                        column: x => x.id_attraction,
                        principalTable: "Attraction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_attraction = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.id);
                    table.ForeignKey(
                        name: "FK_Favorites_Attraction_id_attraction",
                        column: x => x.id_attraction,
                        principalTable: "Attraction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Person_id_user",
                        column: x => x.id_user,
                        principalTable: "Person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rating = table.Column<int>(type: "int", nullable: false),
                    id_attraction = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.id);
                    table.ForeignKey(
                        name: "FK_Review_Attraction_id_attraction",
                        column: x => x.id_attraction,
                        principalTable: "Attraction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Person_id_user",
                        column: x => x.id_user,
                        principalTable: "Person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Rating_id_rating",
                        column: x => x.id_rating,
                        principalTable: "Rating",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_itinerary = table.Column<int>(type: "int", nullable: false),
                    day_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day", x => x.id);
                    table.ForeignKey(
                        name: "FK_Day_Itinerary_id_itinerary",
                        column: x => x.id_itinerary,
                        principalTable: "Itinerary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review_Images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_review = table.Column<int>(type: "int", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    picture_ref = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Review_Images_Review_id_review",
                        column: x => x.id_review,
                        principalTable: "Review",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timeslot",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_attraction = table.Column<int>(type: "int", nullable: true),
                    id_day = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeslot", x => x.id);
                    table.ForeignKey(
                        name: "FK_Timeslot_Attraction_id_attraction",
                        column: x => x.id_attraction,
                        principalTable: "Attraction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Timeslot_Day_id_day",
                        column: x => x.id_day,
                        principalTable: "Day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_id_country",
                table: "Attraction",
                column: "id_country");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_Images_id_attraction",
                table: "Attraction_Images",
                column: "id_attraction");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_Info_id_attraction",
                table: "Attraction_Info",
                column: "id_attraction");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_Info_id_attraction_info_type",
                table: "Attraction_Info",
                column: "id_attraction_info_type");

            migrationBuilder.CreateIndex(
                name: "IX_Day_id_itinerary",
                table: "Day",
                column: "id_itinerary");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_id_attraction",
                table: "Favorites",
                column: "id_attraction");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_id_user",
                table: "Favorites",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Itinerary_id_user",
                table: "Itinerary",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_attraction",
                table: "Review",
                column: "id_attraction");

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_rating",
                table: "Review",
                column: "id_rating");

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_user",
                table: "Review",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Images_id_review",
                table: "Review_Images",
                column: "id_review");

            migrationBuilder.CreateIndex(
                name: "IX_Timeslot_id_attraction",
                table: "Timeslot",
                column: "id_attraction");

            migrationBuilder.CreateIndex(
                name: "IX_Timeslot_id_day",
                table: "Timeslot",
                column: "id_day");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attraction_Images");

            migrationBuilder.DropTable(
                name: "Attraction_Info");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Review_Images");

            migrationBuilder.DropTable(
                name: "Timeslot");

            migrationBuilder.DropTable(
                name: "Attraction_Info_Type");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Day");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Itinerary");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
