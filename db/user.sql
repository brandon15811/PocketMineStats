REVOKE ALL PRIVILEGES ON `stats`.* FROM `stats`@'%';
GRANT CREATE, ALTER, DROP, INSERT, UPDATE, DELETE, SELECT, REFERENCES on `stats`.* TO `stats`@'%' WITH GRANT OPTION;