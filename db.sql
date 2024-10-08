CREATE TABLE IF NOT EXISTS Users (
    user_id SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    salt VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Profiles (
    profile_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    profile_name VARCHAR(255) NOT NULL,
    api_key VARCHAR(255) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE IF NOT EXISTS PersonalInfo (
    personal_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    full_name VARCHAR(255) NULL,
    phone VARCHAR(20) NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE IF NOT EXISTS Stocks (
    stock_id SERIAL PRIMARY KEY,
    profile_id INT NOT NULL,
    warehouse_name VARCHAR(255) NOT NULL,
    product_quantity INT NOT NULL,
    product_name VARCHAR(255) NOT NULL,
    product_sku VARCHAR(255) NOT NULL,
    last_update TIMESTAMP NOT NULL,
    lifetime_minutes INT NULL,
    FOREIGN KEY (profile_id) REFERENCES Profiles(profile_id)
);

CREATE TABLE IF NOT EXISTS Orders (
    order_id SERIAL PRIMARY KEY,
    profile_id INT NOT NULL,
    order_date DATE NOT NULL,
    order_time TIME NOT NULL,
    order_amount DECIMAL(10, 2) NOT NULL,
    order_name VARCHAR(255) NOT NULL,
    order_sku VARCHAR(255) NOT NULL,
    order_brand VARCHAR(255) NOT NULL,
    order_category VARCHAR(255) NOT NULL,
    order_country VARCHAR(255) NOT NULL,
    order_state VARCHAR(255) NOT NULL,
    order_region VARCHAR(255) NOT NULL,
    last_update TIMESTAMP NOT NULL,
    lifetime_minutes INT NULL,
    FOREIGN KEY (profile_id) REFERENCES Profiles(profile_id)
);

CREATE TABLE IF NOT EXISTS Purchases (
    purchase_id SERIAL PRIMARY KEY,
    profile_id INT NOT NULL,
    purchase_date DATE NOT NULL,
    purchase_time TIME NOT NULL,
    purchase_amount DECIMAL(10, 2) NOT NULL,
    purchase_name VARCHAR(255) NOT NULL,
    purchase_sku VARCHAR(255) NOT NULL,
    purchase_brand VARCHAR(255) NOT NULL,
    purchase_category VARCHAR(255) NOT NULL,
    purchase_country VARCHAR(255) NOT NULL,
    purchase_state VARCHAR(255) NOT NULL,
    purchase_region VARCHAR(255) NOT NULL,
    last_update TIMESTAMP NOT NULL,
    lifetime_minutes INT NULL,
    FOREIGN KEY (profile_id) REFERENCES Profiles(profile_id)
);

CREATE TABLE IF NOT EXISTS Payments (
    payment_id SERIAL PRIMARY KEY,
    profile_id INT NOT NULL,
    payment_date DATE NOT NULL,
    payment_status VARCHAR(50) NOT NULL,
    payment_amount DECIMAL(10, 2) NOT NULL,
    last_update TIMESTAMP NOT NULL,
    lifetime_minutes INT NULL,
    FOREIGN KEY (profile_id) REFERENCES Profiles(profile_id)
);