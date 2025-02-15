﻿using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get;  set; }
        public string Password { get; set; }
        [JsonIgnore]
        public Token? Token { get; set; }

        public User(int id, string password, string username, Token token)
        {
            Id = id;
            Username = username;
            Password = password;
            Token = token;
        }

        public User(string username, string password, Token token)
        {
            Username = username;
            Password = password;
            Token = token;
        }
        public User()
        {

        }
        public User(string username,string password)
        {
            this.Username= username;
            this.Password = password;
        }
        public User(int userId, string username, string password)
        {
            this.Id = userId;
            this.Username = username;
            this.Password = password;
        }

    }
}
