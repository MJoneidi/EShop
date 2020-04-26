# EShop
Micro service with RabitMq 

<p>Rabbitmq</p>



Exchange Types<br/> 
   Direct:<br/>
		The routing algorithm behind a direct exchange is simple - a message goes to the queues whose binding key exactly matches the routing key of the message.<br/>
   Fan-out: <br/>
		A fanout exchange routes messages to all of the queues that are bound to it and the routing key is ignored.<br/>
   Topic:<br/>
		Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange.<br/>
		* (star) can substitute for exactly one word.<br/>
		# (hash) can substitute for zero or more words.<br/>
   Hearers:<br/>
		A headers exchange is designed for routing on multiple attributes that are more easily expressed as message headers than a routing key.<br/>

