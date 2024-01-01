https://strimzi.io/quickstarts/

kubectl create namespace kafka

kubectl create -f 'https://strimzi.io/install/latest?namespace=kafka' -n kafka

kubectl apply -f topics.yaml

kubectl describe kafkatopic speech -n kafka

kubectl apply -f service.yaml
kubectl port-forward service/kafka-service -n kafka 9092:9092

