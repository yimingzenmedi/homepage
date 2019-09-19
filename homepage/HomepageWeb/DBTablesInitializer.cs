using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomepageWeb.DAL;

namespace HomepageWeb.BLL
{
    public static class DBTablesInitializer
    {
        public static void InitDBTables()
        {
            string initSqlQurry = @"
            --Check if table UserInfo exists, if not: create 
            IF OBJECT_ID(N'UserInfo') IS null
            BEGIN
            PRINT 'NULL'
            CREATE TABLE [dbo].[UserInfo](
	            [Username] [varchar](50) NOT NULL,
	            [Password] [nchar](32) NOT NULL,
	            [Salt] [nchar](25) NOT NULL,
	            [Email] [nchar](32) NOT NULL,
             CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
            (
	            [Username] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            END
            --Check if table UserSites exists, if not: create 

            IF OBJECT_ID(N'UserSites') IS null
            BEGIN
            PRINT 'NULL'
            CREATE TABLE [dbo].[UserSites](
	            [Username] [varchar](50) NOT NULL,
	            [Sitename] [varchar](200) NOT NULL,
	            [Siteurl] [varchar](500) NOT NULL
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[UserSites]  WITH CHECK ADD  CONSTRAINT [FK_UserSites_UserInfo] FOREIGN KEY([Username])
            REFERENCES [dbo].[UserInfo] ([Username])
            ALTER TABLE [dbo].[UserSites] CHECK CONSTRAINT [FK_UserSites_UserInfo]
            ALTER TABLE [dbo].[UserSites] WITH CHECK ADD CONSTRAINT [IX_UserSites] UNIQUE ([Username],[Sitename],[Siteurl])
            END";

            DBHelper.ExecuteNonQuery(initSqlQurry, null);
        }
    }
}
