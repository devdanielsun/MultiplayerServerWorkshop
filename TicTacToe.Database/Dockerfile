# Use the official MySQL image as a base
FROM mysql:latest

# Set the default user for the MySQL image
USER mysql

# Set environment variables for the new user
ENV MYSQL_ROOT_PASSWORD=root_password
ENV MYSQL_USER=external_user
ENV MYSQL_PASSWORD=password
ENV MYSQL_DATABASE=my_database

# Copy SQL files for schema creation and data insertion
COPY ./01-schema.sql /docker-entrypoint-initdb.d/
COPY ./02-data.sql /docker-entrypoint-initdb.d/

# Expose port 3306 to allow connections to the database
EXPOSE 3306

# Start the MySQL server when the container is run
CMD ["mysqld"]