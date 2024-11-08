CREATE TABLE Locations (
    id INT PRIMARY KEY IDENTITY(1,1),
    title VARCHAR(255) NOT NULL,
    city VARCHAR(100) NOT NULL,
    statec VARCHAR(100),
    country VARCHAR(100) NOT NULL,
    zip VARCHAR(20)
);


CREATE TABLE Departments (
    id INT PRIMARY KEY IDENTITY(1,1),
    title VARCHAR(255) NOT NULL
);

CREATE TABLE Jobs (
    id INT PRIMARY KEY IDENTITY(1,1),
    code VARCHAR(10) UNIQUE NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    locationId INT,
    departmentId INT,
    postedDate DATETIME DEFAULT GETDATE(),
    closingDate DATETIME,
    FOREIGN KEY (locationId) REFERENCES Locations(id),
    FOREIGN KEY (departmentId) REFERENCES Departments(id)
);
