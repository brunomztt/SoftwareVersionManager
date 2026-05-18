-- Script para criar o banco de dados localmente
-- Execute este script no MySQL se não estiver usando Docker

CREATE DATABASE IF NOT EXISTS software_version_manager;
USE software_version_manager;

CREATE TABLE IF NOT EXISTS Softwares (
	Id INT PRIMARY KEY AUTO_INCREMENT,
	Name VARCHAR(255) NOT NULL UNIQUE,
	Description LONGTEXT,
	Developer LONGTEXT,
	CreatedAt DATETIME(6) NOT NULL,
	UpdatedAt DATETIME(6) NOT NULL
) CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS SoftwareVersions (
	Id INT PRIMARY KEY AUTO_INCREMENT,
	SoftwareId INT NOT NULL,
	VersionNumber VARCHAR(255) NOT NULL,
	ReleaseNotes LONGTEXT,
	ReleaseDate DATETIME(6) NOT NULL,
	IsDeprecated BOOLEAN NOT NULL DEFAULT FALSE,
	CreatedAt DATETIME(6) NOT NULL,
	UpdatedAt DATETIME(6) NOT NULL,
	UNIQUE KEY UK_SoftwareVersion (SoftwareId, VersionNumber),
	FOREIGN KEY (SoftwareId) REFERENCES Softwares(Id) ON DELETE CASCADE
) CHARSET=utf8mb4;

CREATE INDEX IX_Softwares_Name ON Softwares(Name);
