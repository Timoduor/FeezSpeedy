import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";

export interface SummaryCardProps {
    title: string;
    value: number | string;
    variant?: "primary" | "success" | "danger" | "warning" | "secondary";
    id?: string;
}

export interface SummaryCardsProps {
    cards: SummaryCardProps[];
}

const SummaryCards: React.FC<SummaryCardsProps> = ({ cards }) => {
    return (
        <div className="row g-3">
            {cards.map((card, idx) => (
                <div className="col-xl-3 col-lg-4 col-md-6" key={idx}>
                    <div className="card shadow-sm border-0 h-100">
                        <div className="card-body">
                            <small className="text-uppercase text-muted fw-semibold">
                                {card.title}
                            </small>
                            <h4 className={`fw-bold mt-2 text-${card.variant ?? "primary"}`}>
                                {typeof card.value === "number"
                                    ? card.value.toLocaleString()
                                    : card.value}
                            </h4>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );


};

export default SummaryCards;