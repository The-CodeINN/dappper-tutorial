DROP TABLE IF EXISTS TutorialAppSchema.Users;

-- IF OBJECT_ID('TutorialAppSchema.Users') IS NOT NULL
--     DROP TABLE TutorialAppSchema.Users;

CREATE TABLE TutorialAppSchema.Users
(
    UserId INT IDENTITY(1, 1) PRIMARY KEY
    , FirstName NVARCHAR(50)
    , LastName NVARCHAR(50)
    , Email NVARCHAR(50) UNIQUE
    , Gender NVARCHAR(50)
    , Active BIT
);

-- Run this script in your SQL Server Management Studio to create the table
        -- CREATE DATABASE DotNetCourseDatabase;
        -- GO

        -- USE DotNetCourseDatabase; 
        -- GO

        -- CREATE SCHEMA TutorialAppSchema
        -- GO


-- Run this script to view the table

        -- SELECT  [UserId]
        --         , [FirstName]
        --         , [LastName]
        --         , [Email]
        --         , [Gender]
        --         , [Active]
        --   FROM  TutorialAppSchema.Users;