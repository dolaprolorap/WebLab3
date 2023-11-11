﻿using System.ComponentModel.DataAnnotations;

namespace backend.Models.DB
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(16)]
        [Required]
        public string Name { get; set; }
        [MaxLength(16)]
        [Required]
        public string Password { get; set; }

        public User(Guid guid, string name, string password)
        {
            Id = guid;
            Name = name;
            Password = password;
        }
    }
}