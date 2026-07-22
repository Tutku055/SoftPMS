namespace SoftPMS.Domain.Enums;

public enum DocumentType
{
    // ==========================================
    // COMMON / GENERAL
    // ==========================================
    /// <summary>
    /// Default fallback for any unclassified document.
    /// </summary>
    Other = 0,

    // ==========================================
    // EMPLOYEE MODULE DOCUMENTS
    // ==========================================
    /// <summary>
    /// Identification cards, passport, driver's license, birth certificate.
    /// </summary>
    Identification = 1,

    /// <summary>
    /// Employment contract, NDA, non-compete agreements, offer letters.
    /// </summary>
    Contract = 2,

    /// <summary>
    /// Medical reports, periodic health clearance, disability records, OSHA forms.
    /// </summary>
    HealthRecord = 3,

    /// <summary>
    /// University diploma, certificates, training completion documents, language proficiency test results.
    /// </summary>
    Certificate = 4,

    /// <summary>
    /// Resume, CV, cover letters, portfolio documents.
    /// </summary>
    Resume = 5,

    /// <summary>
    /// Criminal record check, background check clearance, reference check forms.
    /// </summary>
    BackgroundCheck = 6,

    /// <summary>
    /// Performance reviews, self-assessments, KPI evaluations, 360-degree feedback reports.
    /// </summary>
    PerformanceReview = 7,

    /// <summary>
    /// Tax forms (e.g., W-4, W-2), bank account details for payroll, compensation history forms.
    /// </summary>
    PayrollAndTax = 8,

    /// <summary>
    /// Resignation letters, exit interview notes, termination notices, severance agreements.
    /// </summary>
    Offboarding = 9,

    // ==========================================
    // DEPARTMENT MODULE DOCUMENTS
    // ==========================================
    /// <summary>
    /// Departmental internal policies, code of conduct, standard operating procedures (SOPs).
    /// </summary>
    Policy = 100,

    /// <summary>
    /// Annual/quarterly department budget plans, financial forecast documents, expenditure reports.
    /// </summary>
    BudgetReport = 101,

    /// <summary>
    /// Departmental org charts, role hierarchy maps, headcount planning sheets.
    /// </summary>
    OrgStructure = 102,

    /// <summary>
    /// Compliance audit reports, ISO certification docs, safety/risk assessment reports.
    /// </summary>
    ComplianceAndAudit = 103,

    /// <summary>
    /// Departmental meeting minutes, strategic roadmaps, quarterly goals (OKRs).
    /// </summary>
    StrategicPlan = 104
}
