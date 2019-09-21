# ZoBo - Zone Based Firewall Bootstrapper for VyOS

ZoBo helps you bootstrap your VyOS Zone-Based Firewall through an easy config file to get you up and running asap.

## Running

### Docker
```sh
docker run --rm -it -v $(pwd)/zones.yaml:/app/zones.yaml ebrithil/zobo
```

### From Source

*Note*: You need to have the [Dotnet Core SDK](https://dotnet.microsoft.com/download) installed!

```sh
git clone https://github.com/Ebrithil95/zobo.git
cd zobo
dotnet restore
dotnet run
```

## Config Syntax

TODO

## Example

### zones.yaml
```yaml
zones:
  - wan
  - local
  - lan
  - mgmt

definitions:
  wan:
    interface: "eth0"
    description: "WAN Network"
    allow_ping_to: "local"
    allow_traffic_to:
      local:
        ports: ["22"]
  local:
    description: "Local Zone"
    is_local_zone: true
    allow_traffic_to: "*"
  lan:
    description: "LAN Network"
    interface: "eth1"
    allow_traffic_to:
      local:
        # Whitelist DNS
        ports: ["53/tcp_udp"]
      wan:
  mgmt:
    description: "Management Network"
    interface: "eth1.1"
    allow_ping_to: "*"
    allow_traffic_to:
      # Allow SSH to any zone
      "*":
        ports: ["22"]
      wan:
```

### Output
```bash
set zone-policy zone 'wan' default-action 'drop'
set zone-policy zone 'wan' description 'WAN Network'
set zone-policy zone 'wan' interface 'eth0'
set zone-policy zone 'local' default-action 'drop'
set zone-policy zone 'local' description 'Local Zone'
set zone-policy zone 'local' local-zone
set zone-policy zone 'lan' default-action 'drop'
set zone-policy zone 'lan' description 'LAN Network'
set zone-policy zone 'lan' interface 'eth1'
set zone-policy zone 'mgmt' default-action 'drop'
set zone-policy zone 'mgmt' description 'Management Network'
set zone-policy zone 'mgmt' interface 'eth1.1'
set firewall name 'wan-local' default-action drop
set firewall name 'wan-local' enable-default-log
set firewall name 'wan-local' rule 10 action accept
set firewall name 'wan-local' rule 10 state established enable
set firewall name 'wan-local' rule 10 state related enable
set firewall name 'wan-local' rule 10 description 'Allow established connections'
set firewall name 'wan-local' rule 15 action accept
set firewall name 'wan-local' rule 15 protocol icmp
set firewall name 'wan-local' rule 15 description 'Allow pings'
set firewall name 'wan-local' rule 50 action accept
set firewall name 'wan-local' rule 50 protocol tcp
set firewall name 'wan-local' rule 50 destination port 22
set zone-policy zone local from wan firewall name wan-local
set firewall name 'wan-lan' default-action drop
set firewall name 'wan-lan' enable-default-log
set firewall name 'wan-lan' rule 10 action accept
set firewall name 'wan-lan' rule 10 state established enable
set firewall name 'wan-lan' rule 10 state related enable
set firewall name 'wan-lan' rule 10 description 'Allow established connections'
set zone-policy zone lan from wan firewall name wan-lan
set firewall name 'wan-mgmt' default-action drop
set firewall name 'wan-mgmt' enable-default-log
set firewall name 'wan-mgmt' rule 10 action accept
set firewall name 'wan-mgmt' rule 10 state established enable
set firewall name 'wan-mgmt' rule 10 state related enable
set firewall name 'wan-mgmt' rule 10 description 'Allow established connections'
set zone-policy zone mgmt from wan firewall name wan-mgmt
set firewall name 'local-wan' default-action accept
set firewall name 'local-wan' enable-default-log
set firewall name 'local-wan' rule 10 action accept
set firewall name 'local-wan' rule 10 state established enable
set firewall name 'local-wan' rule 10 state related enable
set firewall name 'local-wan' rule 10 description 'Allow established connections'
set zone-policy zone wan from local firewall name local-wan
set firewall name 'local-lan' default-action accept
set firewall name 'local-lan' enable-default-log
set firewall name 'local-lan' rule 10 action accept
set firewall name 'local-lan' rule 10 state established enable
set firewall name 'local-lan' rule 10 state related enable
set firewall name 'local-lan' rule 10 description 'Allow established connections'
set zone-policy zone lan from local firewall name local-lan
set firewall name 'local-mgmt' default-action accept
set firewall name 'local-mgmt' enable-default-log
set firewall name 'local-mgmt' rule 10 action accept
set firewall name 'local-mgmt' rule 10 state established enable
set firewall name 'local-mgmt' rule 10 state related enable
set firewall name 'local-mgmt' rule 10 description 'Allow established connections'
set zone-policy zone mgmt from local firewall name local-mgmt
set firewall name 'lan-wan' default-action accept
set firewall name 'lan-wan' enable-default-log
set firewall name 'lan-wan' rule 10 action accept
set firewall name 'lan-wan' rule 10 state established enable
set firewall name 'lan-wan' rule 10 state related enable
set firewall name 'lan-wan' rule 10 description 'Allow established connections'
set zone-policy zone wan from lan firewall name lan-wan
set firewall name 'lan-local' default-action drop
set firewall name 'lan-local' enable-default-log
set firewall name 'lan-local' rule 10 action accept
set firewall name 'lan-local' rule 10 state established enable
set firewall name 'lan-local' rule 10 state related enable
set firewall name 'lan-local' rule 10 description 'Allow established connections'
set firewall name 'lan-local' rule 50 action accept
set firewall name 'lan-local' rule 50 protocol tcp_udp
set firewall name 'lan-local' rule 50 destination port 53
set zone-policy zone local from lan firewall name lan-local
set firewall name 'lan-mgmt' default-action drop
set firewall name 'lan-mgmt' enable-default-log
set firewall name 'lan-mgmt' rule 10 action accept
set firewall name 'lan-mgmt' rule 10 state established enable
set firewall name 'lan-mgmt' rule 10 state related enable
set firewall name 'lan-mgmt' rule 10 description 'Allow established connections'
set zone-policy zone mgmt from lan firewall name lan-mgmt
set firewall name 'mgmt-wan' default-action accept
set firewall name 'mgmt-wan' enable-default-log
set firewall name 'mgmt-wan' rule 10 action accept
set firewall name 'mgmt-wan' rule 10 state established enable
set firewall name 'mgmt-wan' rule 10 state related enable
set firewall name 'mgmt-wan' rule 10 description 'Allow established connections'
set firewall name 'mgmt-wan' rule 15 action accept
set firewall name 'mgmt-wan' rule 15 protocol icmp
set firewall name 'mgmt-wan' rule 15 description 'Allow pings'
set firewall name 'mgmt-wan' rule 50 action accept
set firewall name 'mgmt-wan' rule 50 protocol tcp
set firewall name 'mgmt-wan' rule 50 destination port 22
set zone-policy zone wan from mgmt firewall name mgmt-wan
set firewall name 'mgmt-local' default-action drop
set firewall name 'mgmt-local' enable-default-log
set firewall name 'mgmt-local' rule 10 action accept
set firewall name 'mgmt-local' rule 10 state established enable
set firewall name 'mgmt-local' rule 10 state related enable
set firewall name 'mgmt-local' rule 10 description 'Allow established connections'
set firewall name 'mgmt-local' rule 15 action accept
set firewall name 'mgmt-local' rule 15 protocol icmp
set firewall name 'mgmt-local' rule 15 description 'Allow pings'
set firewall name 'mgmt-local' rule 50 action accept
set firewall name 'mgmt-local' rule 50 protocol tcp
set firewall name 'mgmt-local' rule 50 destination port 22
set zone-policy zone local from mgmt firewall name mgmt-local
set firewall name 'mgmt-lan' default-action drop
set firewall name 'mgmt-lan' enable-default-log
set firewall name 'mgmt-lan' rule 10 action accept
set firewall name 'mgmt-lan' rule 10 state established enable
set firewall name 'mgmt-lan' rule 10 state related enable
set firewall name 'mgmt-lan' rule 10 description 'Allow established connections'
set firewall name 'mgmt-lan' rule 15 action accept
set firewall name 'mgmt-lan' rule 15 protocol icmp
set firewall name 'mgmt-lan' rule 15 description 'Allow pings'
set firewall name 'mgmt-lan' rule 50 action accept
set firewall name 'mgmt-lan' rule 50 protocol tcp
set firewall name 'mgmt-lan' rule 50 destination port 22
set zone-policy zone lan from mgmt firewall name mgmt-lan
```

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Ebrithil95/zobo/tags).

## License

This project is licensed under the AGPLv3 License - see the [LICENSE](LICENSE) file for details.