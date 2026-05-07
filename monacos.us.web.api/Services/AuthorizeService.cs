using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using monacos.us.web.api.Models;
using System.Data;
using System.Data.Common;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace monacos.us.web.api.Services
{
    public class AuthorizeService : IAuthorize
    {

        readonly string _dBConnectionString;
        readonly  IConfiguration _configuration;


        public AuthorizeService(string dBConnectionString, IConfiguration configuration)
        {

            this._dBConnectionString = dBConnectionString;
            this._configuration = configuration;

        }

        public RegisteredUserDTO Register(UserDTO register_request)
        {

            RegisteredUserDTO user = new RegisteredUserDTO();
            Int32 UserID = 0;

            try
            {

                // Create a Hash of the Password

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(register_request.Password);

                user.Username = register_request.Username;
                user.PasswordHash = passwordHash;

                UserID = AddUser(user);

                user.UserID = UserID;


            }
            catch (Exception ex)
            {

                throw new Exception("AuthorizeService::Register Error: " + ex.Message);

            }

            return user;

        }

        public LoggedInUserDTO Login(UserDTO login_request)
        {

            RegisteredUserDTO? user = null;
            string jSONWebToken = null;
            LoggedInUserDTO? loggedInUser = null;



            try
            {

                // Database Lookup if User Exists
                user = GetUser(login_request.Username);

                if (user == null)
                {

                    // User Not Found 
                    return null;

                }
                

                // Verify the Password against its Hash

                if (!BCrypt.Net.BCrypt.Verify(login_request.Password, user.PasswordHash))
                {
                    // Wrong Password
                    return null;
                }

                // Create JSON Web Token
                jSONWebToken = CreateToken(user);



                // Create Logged In User DTO
                loggedInUser = new LoggedInUserDTO();

                loggedInUser.UserName = user.Username;
                loggedInUser.Token = jSONWebToken;
                loggedInUser.UserID = user.UserID;

                // Query User Role/s
                loggedInUser.UserRoles = this.GetUserRoles(user.UserID);




            }
            catch (Exception ex)
            {

                throw new Exception("AuthorizeService::Login Error: " + ex.Message);

                
            }

            return loggedInUser;


        }


        private RegisteredUserDTO GetUser(string username)
        {

            SqlConnection objSQLConnection = null;
            SqlCommand objSQLCommand = null;
            SqlDataAdapter objDataAdapter = null;
            string strSQL = string.Empty;
            
            DataSet dataSet = null;
            RegisteredUserDTO? objUser = null;

            try
            {

               
                using (objSQLConnection = new SqlConnection(this._dBConnectionString))
                {


                    strSQL = "sp_GetUser";
                    objSQLCommand = new SqlCommand(strSQL, objSQLConnection);
                    objSQLCommand.CommandType = CommandType.StoredProcedure;


                    objSQLCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 50);

                    objSQLCommand.Parameters["@UserName"].Value = username;

                    objDataAdapter = new SqlDataAdapter(objSQLCommand);

                    dataSet = new DataSet();

                    objDataAdapter.Fill(dataSet);


                    if (dataSet.Tables[0].Rows.Count > 0)
                    {

                        objUser = new RegisteredUserDTO();

                        objUser.Username = Convert.ToString(dataSet.Tables[0].Rows[0]["User_Name"]);
                        objUser.PasswordHash = Convert.ToString(dataSet.Tables[0].Rows[0]["User_Password_Hash"]);
                        objUser.UserID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["User_ID"]);

                    }
                    else
                    {
                        objUser = null;

                    }

                }

                
            }
            catch (Exception ex)
            {

                throw new Exception("AuthorizeService::GetUser Error: " + ex.Message);

            }

            return objUser;

        }


        private IList<Int32> GetUserRoles(Int32 UserID)
        {

            SqlConnection objSQLConnection = null;
            SqlCommand objSQLCommand = null;
            SqlDataAdapter objDataAdapter = null;
            string strSQL = string.Empty;
            List<int> userRoleList = new List<int>();
            DataSet dataSet = null;
            

            try
            {


                using (objSQLConnection = new SqlConnection(this._dBConnectionString))
                {


                    strSQL = "sp_GetUserRoles";
                    objSQLCommand = new SqlCommand(strSQL, objSQLConnection);
                    objSQLCommand.CommandType = CommandType.StoredProcedure;


                    objSQLCommand.Parameters.Add("@UserID", SqlDbType.Int);

                    objSQLCommand.Parameters["@UserID"].Value = UserID;

                    objDataAdapter = new SqlDataAdapter(objSQLCommand);

                    dataSet = new DataSet();

                    objDataAdapter.Fill(dataSet);

                    foreach( DataRow row in dataSet.Tables[0].Rows )
                    {

                        userRoleList.Add(Convert.ToInt32(row["Resource_Role_ID"]));

                    }
                    
                }


            }
            catch (Exception ex)
            {

                throw new Exception("AuthorizeService::GetUser Error: " + ex.Message);

            }

            return userRoleList;

        }



        // Generate JWT Token
        private string CreateToken(RegisteredUserDTO user)
        {

            try
            {


                // JWT Claims for Payload


                // First Claim is User Name and Password Hash
                // Not Using Second Claim of Role
                List<Claim> claims = new List<Claim>
            {

                new Claim(ClaimTypes.Name, user.Username)
                //new Claim(ClaimTypes.Role, "Admin")

            };

                // Secret Key can be generated from online service JWTSecrets or Other Secret Key Generator.

                // Create Issuer Signing Security Key From Secret Key  - used in Signature segment of JWT Token Creation

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:TokenSecretKey").Value!));

                // Create a Digital Signature for the Signing Security Key 

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


                // Generate JWT Token

                var token = new JwtSecurityToken(

                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred

                    );



                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;

            }
            catch (Exception ex)
            {

                throw new Exception("CreateToken::GetUser Error: " + ex.Message);

            }

        }

        private Int32 AddUser(RegisteredUserDTO user)
        {


            SqlConnection objSQLConnection = null;
            SqlCommand objSQLCommand = null;

            string strSQL = string.Empty;
            string strConnection = string.Empty;
            Int32 UserID = 0;

            try
            {

                using (objSQLConnection = new SqlConnection(this._dBConnectionString))
                {
                    objSQLConnection.Open();

                    strSQL = "sp_InsertUser";
                    objSQLCommand = new SqlCommand(strSQL, objSQLConnection);
                    objSQLCommand.CommandType = CommandType.StoredProcedure;


                    objSQLCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 50);
                    objSQLCommand.Parameters["@UserName"].Value = user.Username;

                    objSQLCommand.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 1000);
                    objSQLCommand.Parameters["@PasswordHash"].Value = user.PasswordHash;

                    UserID = (Int32)objSQLCommand.ExecuteScalar();

                }

                return UserID;

            }
            catch (Exception ex)
            {


                throw new Exception("AddUser::AddUser Error: " + ex.Message);


            }


        }


    }
}
