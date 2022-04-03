import React, { Component } from 'react';

export class EditCompanyForm extends Component {
    static displayName = EditCompanyForm.name;

    constructor(props) {
        super(props);

        this.state = {
            name: '',
            token: sessionStorage.getItem('token')
        };

        this.updateCompany = this.updateCompany.bind(this);
        this.handleName = this.handleName.bind(this);
    }

    handleName = event => {
        this.setState({name: event.target.value});
    }

    async updateCompany(event) {
        event.preventDefault();

        await fetch(`../api/companies/${window.location.pathname.split('/')[3]}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .then(() => window.location.href = '/companies')
            .catch(error => alert(error.message));
    }

    render() {
        return <form onSubmit={this.updateCompany}>
            <div className="form-group mb-3">
                <label htmlFor="name" className="control-label">Name:</label>
                <input id="name" className="form-control" type="text" value={this.state.name} onChange={this.handleName} /> 
            </div>

            <input className="btn btn-primary" type="submit" value="Update" />
        </form>;
    }
}