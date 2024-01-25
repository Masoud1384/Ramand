exec dbo.InsertUser @username = 'alireza', @password = 'mohamadi';
exec dbo.SelectUser @Id = 5;
-- exec dbo.UpdateUser @Id = 1, @username = 'masoud' , @password = 'shooshtari';
-- exec dbo.DeleteUser @Id = 1;