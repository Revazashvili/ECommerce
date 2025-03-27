# setup ordering outbox
curl --location --request PUT 'http://localhost:8083/connectors/ordering-outbox-connector/config'  \
--header 'Content-Type: application/json' \
--data '
{
  "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
  "tasks.max": "1",
  "database.hostname": "postgres",
  "database.port": "5432",
  "database.user": "postgres",
  "database.password": "mysecretpassword",
  "database.dbname": "ordering",
  "database.server.name": "postgres",
  "schema.include.list": "outbox",
  "table.include.list": "outbox.outbox_messages",
  "tombstones.on.delete": "false",
  "transforms": "outbox",
  "transforms.outbox.type": "io.debezium.transforms.outbox.EventRouter",
  "transforms.outbox.table.field.event.id": "id",
  "transforms.outbox.table.field.event.key": "aggregate_id",
  "transforms.outbox.table.field.event.payload": "payload",
  "transforms.outbox.table.expand.json.payload": true,
  "transforms.outbox.route.by.field": "topic",
  "transforms.outbox.route.topic.replacement": "ordering.${routedByValue}",
  "value.converter":"org.apache.kafka.connect.json.JsonConverter",
  "value.converter.schemas.enable": false,
  "plugin.name": "pgoutput",
  "topic.prefix": "ordering",
  "publication.autocreate.mode": "filtered",
  "slot.name": "ordering_slot"
}'


# setup products outbox
curl --location --request PUT 'http://localhost:8083/connectors/products-outbox-connector/config'  \
--header 'Content-Type: application/json' \
--data '
{
  "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
  "tasks.max": "1",
  "database.hostname": "postgres",
  "database.port": "5432",
  "database.user": "postgres",
  "database.password": "mysecretpassword",
  "database.dbname": "products",
  "database.server.name": "postgres",
  "schema.include.list": "outbox",
  "table.include.list": "outbox.outbox_messages",
  "tombstones.on.delete": "false",
  "transforms": "outbox",
  "transforms.outbox.type": "io.debezium.transforms.outbox.EventRouter",
  "transforms.outbox.table.field.event.id": "id",
  "transforms.outbox.table.field.event.key": "aggregate_id",
  "transforms.outbox.table.field.event.payload": "payload",
  "transforms.outbox.table.expand.json.payload": true,
  "transforms.outbox.route.by.field": "topic",
  "transforms.outbox.route.topic.replacement": "products.${routedByValue}",
  "value.converter":"org.apache.kafka.connect.json.JsonConverter",
  "value.converter.schemas.enable": false,
  "plugin.name": "pgoutput",
  "topic.prefix": "products",
  "publication.autocreate.mode": "filtered",
  "slot.name": "products_slot"
}'

# setup payments outbox
curl --location --request PUT 'http://localhost:8083/connectors/payments-outbox-connector/config'  \
--header 'Content-Type: application/json' \
--data '
{
  "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
  "tasks.max": "1",
  "database.hostname": "postgres",
  "database.port": "5432",
  "database.user": "postgres",
  "database.password": "mysecretpassword",
  "database.dbname": "payment",
  "database.server.name": "postgres",
  "schema.include.list": "outbox",
  "table.include.list": "outbox.outbox_messages",
  "tombstones.on.delete": "false",
  "transforms": "outbox",
  "transforms.outbox.type": "io.debezium.transforms.outbox.EventRouter",
  "transforms.outbox.table.field.event.id": "id",
  "transforms.outbox.table.field.event.key": "aggregate_id",
  "transforms.outbox.table.field.event.payload": "payload",
  "transforms.outbox.table.expand.json.payload": true,
  "transforms.outbox.route.by.field": "topic",
  "transforms.outbox.route.topic.replacement": "payments.${routedByValue}",
  "value.converter":"org.apache.kafka.connect.json.JsonConverter",
  "value.converter.schemas.enable": false,
  "plugin.name": "pgoutput",
  "topic.prefix": "payments",
  "publication.autocreate.mode": "filtered",
  "slot.name": "payments_slot"
}'