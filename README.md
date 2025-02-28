Proposed Coooperator table refactoring
-------------------------

Notes
-----
1. Assume that all tables will use whatever audit fields (created, owned, dates) are part of GRINext schema.
2. Associative tables assume a type_code field that describes the association -- ex., Rollo Tomasi linked to site NC7 as "CURATOR", Lisa Burke linked to site NC& as "ADMIN" (person with resp. for managing site curators and updating contact info)
3. User account/group/authorization tables intentionally omitted, given discussion of what authentication/auth mechanism will be used. Were we to retain existing tables, a person_sys_user_map table would connect person to sys_user.
4. One approach to implementation along the lines of expand-contract/parallel change could be an interim cooperator_person_map table, somewhat similar to code approach Cullen has devised. 

Questions/For Discussion
-------------------------
1. Some questions about person.status:
   a. Currently, status seems to be part of the workflow that's behind the duplication/over-storage of data: for instance, allowing coop records to be more or less soft-archived while allowing "research" into past orders to be linked to requestor info.. What is the policy behind retaining historical order information? How mucn of this information is needed for stats related to accessions per year/FY/etc.?
   b. If we need info such as "accessions by crop requested in the southwest US," for instance, there must be a way to store the location of the originator of the request without retaining "cooperator" info. A use for geo data/polygons?

```mermaid

---
Proposed Cooperator Refactoring, ERD
---
erDiagram

person {
    INT person_id PK  
    TEXT preferred_pronouns
    TEXT salutation  
    TEXT first_name
    TEXT middle_name
    TEXT last_name
    TEXT phone_number
    TEXT email_address
    TEXT status_code "Ex. ACTIVE, INACTIVE, PENDING"
}

address {
    INT address_id PK
    TEXT line_1
    TEXT line_2
    TEXT line_3
    TEXT city
    TEXT state_province
    TEXT country_code
    TEXT postal_code
    TEXT polygon
}

site {
    INT site_id PK
    TEXT abreviation
    TEXT name
    TEXT fao_institure_number
    INT is_internal "Q: How used? Needed?"
    INT is_distrib "Q: How used? Needed?"
    int type_id "1=SEED, 2-CLONAL, 3=MIXED"
}

site_address_map {
    INT site_id PK, FK
    INT address_id PK, FK
    TEXT type_code "Ex. CONTACT, other?"
}

site_person_map {
    INT person_id PK,FK
    INT site_id PK,FK
    TEXT type_code "Ex. ADMIN, CURATOR"
}

site ||--o{ site_address_map : can_have
person ||--o{ site_address_map : can_be_mapped

person_address_map {
    INT person_id PK,FK
    INT address_id PK,FK
    TEXT type_code "Ex. PRIMARY, SHIPPING"
}

organization {
    INT organization_id PK
    TEXT name
    TEXT url
}

organization_address_map {
    INT organization_id PK,FK
    INT address_id PK,FK
    TEXT type_code "Ex. PRIMARY -- needed?"
}

person ||--o{ person_address_map : can_have
address ||--o{ person_address_map : can_be_mapped

person ||--o{ organization_address_map : is_part_of
organization ||--o{ organization_address_map : has_as_member

```
