# LogCorner.EduSync.Speech.ServiceBus

# start zookeeper-server
zookeeper-server-start.bat  config\zookeeper.properties

#start kafka-server
kafka-server-start.bat  config\server.properties

# create a topic first_topic  
kafka-topics --zookeeper 127.0.0.1:2181  --topic speech --create  --partitions 3  --replication-factor 1
kafka-topics --zookeeper 127.0.0.1:2181  --topic eventbus --create  --partitions 3  --replication-factor 1

kafka-topics --zookeeper 127.0.0.1:2181  --topic second_topic  --create  --partitions 3  --replication-factor 1

#list topic 
kafka-topics --zookeeper 127.0.0.1:2181  --list

#describe topic 
kafka-topics --zookeeper 127.0.0.1:2181  --topic first_topic  --describe

#list topic 
kafka-topics --zookeeper 127.0.0.1:2181  --topic first_topic  --describe

#delete topic
#list topic 
kafka-topics --zookeeper 127.0.0.1:2181  --topic first_topic  --delete

#producer
kafka-console-producer
kafka-console-producer --broker-list 127.0.0.1:9092  --topic first_topic  

#consume
kafka-console-consumer --bootstrap-server 127.0.0.1:9092  --topic first_topic  

#consume from beginning
kafka-console-consumer --bootstrap-server 127.0.0.1:9092  --topic first_topic  --from-beginning





##ELASTIC SEARCH

delete speechindex

GET speechindex/_search
{
  "query": {
    "match_all": {}
  }
}
curl -X GET "http://localhost:9200/speech/_search?pretty"

#KIBANA
bin/kibana
http://localhost:5601

http://localhost:5601/app/dev_tools#/console


SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAV =>  https://dev.azure.com/logcorner-workshop/_git/EventSourcingCQRS-mongo-event-store

docker network create speech_network

ng serve --configuration=docker



kubectl -n kafka run kafka-producer -ti --image=quay.io/strimzi/kafka:0.26.0-kafka-3.0.0 --rm=true --restart=Never -- bin/kafka-console-producer.sh --broker-list logconer-cluster-kafka-bootstrap.kafka:9092 --topic speech


kubectl -n kafka run kafka-consumer -ti --image=quay.io/strimzi/kafka:0.26.0-kafka-3.0.0 --rm=true --restart=Never -- bin/kafka-console-consumer.sh --bootstrap-server logconer-cluster-kafka-bootstrap.kafka:9092 --topic speech --from-beginning