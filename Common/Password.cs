﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class Password
    {
        public Password(string pass)
        {
            if (Regex.IsMatch(pass, "^.{8,18}$"))
            {
                var md5Hasher = MD5.Create();

                var data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(pass));

                var sBuilder = new StringBuilder();

                foreach (var t in data)
                    sBuilder.Append(t.ToString("x2"));

                // Return the hexadecimal string.
                Hash = sBuilder.ToString();
            }
            else
            {
                throw new ArgumentException("Password does not satisfy security requirements");
            }
        }

        public Password()
        {
        }

        public virtual string Hash { get; protected set; }

        public static Password FromPlainString(string value)
        {
            Require.NotEmpty(value, nameof(value));
            return new Password {Hash = value};
        }

        public Password GetHashed()
        {
            return new Password(Hash);
        }

        protected bool Equals(Password other)
        {
            return string.Equals(Hash, other.Hash);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Password) obj);
        }

        public override int GetHashCode()
        {
            return Hash != null ? Hash.GetHashCode() : 0;
        }

        public static bool IsStringCorrectPassword(string passwordToCheck)
        {
            return Regex.IsMatch(passwordToCheck, "^.{8,18}$");
        }
    }
}