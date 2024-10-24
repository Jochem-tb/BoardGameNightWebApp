﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Import this namespace
using System;
using System.IO; // Import this namespace for Path.Combine

namespace BGN.Infrastructure
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
            string connectionString;

            // Check for the environment variable
            var environmentConnectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING");

            if (!string.IsNullOrEmpty(environmentConnectionString))
            {
                connectionString = environmentConnectionString;
            }
            else
            {
                // Load the connection string from appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
                    .AddJsonFile("appsettings.json") // Load the appsettings.json file
                    .Build();

                // Get the connection string from the configuration
                connectionString = configuration.GetConnectionString("BGN_Accounts"); // Adjust the name as needed
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}