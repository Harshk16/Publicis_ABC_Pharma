# Abc Pharmacy Management System

## 📖 Overview
Abc Pharmacy is a full‑stack application built with **ASP.NET Core Web API** and **Angular**.  
It allows users to manage medicines, search by name, and record sales.

### Features
- View list of medicines with expiry and stock indicators
- Add new medicines
- Search medicines by name
- Buy medicines (quantity reduces from stock)
- Sales are logged in a separate record
- Soft delete (medicines marked inactive, not hard deleted)

---

## 🚀 How to Run

### 1. Start the API
Navigate to the API project folder:

```bash
cd Abc_Pharmacy.Web.Api
dotnet run


cd Abc_Pharmacy.Client
ng serve
