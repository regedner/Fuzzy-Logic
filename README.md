# Fuzzy Logic Washing Machine Controller

## Overview
This project implements a Fuzzy Logic Controller for a washing machine, developed in C# with a Windows Forms interface. The system uses fuzzy logic to determine optimal washing parameters (spin speed, duration, and detergent amount) based on three input variables: fabric sensitivity, laundry amount, and dirtiness level. The controller employs the Mamdani inference method for fuzzy reasoning and provides both centroid and weighted average defuzzification techniques.

## Features
- **Fuzzy Sets:** Supports triangular and trapezoidal membership functions for input and output variables.
- **Fuzzy Rules:** Configurable rules to map input conditions (sensitivity, amount, dirtiness) to output parameters (spin speed, duration, detergent).
- **Mamdani Inference:** Computes output values using the Mamdani fuzzy inference system.
- **Defuzzification:** Implements both centroid and weighted average methods to derive crisp output values.
- **Graphical Interface:** Visualizes fuzzy sets, membership degrees, and clipped output areas using charts.
- **Interactive UI:** Allows users to adjust input values via sliders or numeric inputs and view real-time results.
- **Rule Visualization:** Displays active rules and their firing strengths in a data grid, with highlighted active rules.

## Project Structure
- **FuzzySet.cs:** Defines the `FuzzySet` class for handling triangular and trapezoidal membership functions and centroid calculations.
- **FuzzyRule.cs:** Defines the `FuzzyRule` class to represent rules mapping inputs to outputs.
- **FuzzyEngine.cs:** Implements the core fuzzy logic engine, including Mamdani inference, membership calculations, and defuzzification.
- **Form1.cs:** Contains the Windows Forms UI logic, including chart rendering, input handling, and result display.
- **FuzzyData.cs (assumed):** Contains predefined fuzzy sets and rules (not shown in the provided code but referenced).

## Prerequisites
- **.NET Framework:** Ensure you have .NET Framework installed (version compatible with the project, e.g., .NET Framework 4.8).
- **Visual Studio:** Recommended for building and running the project.
- **System.Windows.Forms.DataVisualization:** Required for chart rendering.

## Screenshot

<img width="1920" height="1030" alt="fuzzy" src="https://github.com/user-attachments/assets/3b6e29b2-1d34-43eb-a5ca-89aba4bc3490" />

