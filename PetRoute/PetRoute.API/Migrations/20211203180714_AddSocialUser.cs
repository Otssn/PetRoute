using Microsoft.EntityFrameworkCore.Migrations;

namespace PetRoute.API.Migrations
{
    public partial class AddSocialUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pet_AspNetUsers_UserId",
                table: "pet");

            migrationBuilder.DropForeignKey(
                name: "FK_photoPets_pet_PetId",
                table: "photoPets");

            migrationBuilder.RenameColumn(
                name: "PetId",
                table: "photoPets",
                newName: "petId");

            migrationBuilder.RenameIndex(
                name: "IX_photoPets_PetId",
                table: "photoPets",
                newName: "IX_photoPets_petId");

            migrationBuilder.AlterColumn<int>(
                name: "petId",
                table: "photoPets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "pet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "pet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "pet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialImageURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "logerType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_pet_AspNetUsers_UserId",
                table: "pet",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_photoPets_pet_petId",
                table: "photoPets",
                column: "petId",
                principalTable: "pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pet_AspNetUsers_UserId",
                table: "pet");

            migrationBuilder.DropForeignKey(
                name: "FK_photoPets_pet_petId",
                table: "photoPets");

            migrationBuilder.DropColumn(
                name: "SocialImageURL",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "logerType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "petId",
                table: "photoPets",
                newName: "PetId");

            migrationBuilder.RenameIndex(
                name: "IX_photoPets_petId",
                table: "photoPets",
                newName: "IX_photoPets_PetId");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "photoPets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "pet",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "pet",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "pet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_pet_AspNetUsers_UserId",
                table: "pet",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_photoPets_pet_PetId",
                table: "photoPets",
                column: "PetId",
                principalTable: "pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
