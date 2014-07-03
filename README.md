redis-topshelf
==============
redis-topshelf is a simple windows service installer via using top-shelf(http://topshelf-project.com/) to wrap the redis console window as a 
windows service.

How to install redis windows service by redis-topshelf ?

1.Edit the config file redis-topshelf.exe.config.

    <add key="redis.service.name" value="RedisService"/>
		<add key="redis.service.displayname" value="Redis Service"/>
		<add key="redis.service.description" value="Running redis in background."/>
		<add key="redis.server" value="redis-server.exe"/>
		<add key="redis.conf" value="redis.windows.conf"/>
		<add key="redis.cli" value="redis-cli.exe"/>
		<add key="redis.path" value="D:\Cluster\redis-2.8.9"/>
		
		The config key description as below:
		redis.service.name: the windows sevice name.
		redis.service.displayname: the windows service display name.
		redis.service.description: the windows service desription.
		redis.server: the redis server console file name.
		redis.conf: the redis server config file name.
		redis.cli: the redis client file name.
		redis.path:the redis root directory.
		
		One more important thing is to assign the key "redis.path"...
		
2.Running the bat file install.bat to install the windows service.
  Uninstalling the windows service using the file uninstall.bat.
