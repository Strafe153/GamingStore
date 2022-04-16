import React, { Component } from 'react';
import { base64ToArray } from '../modules/converter';
import { formErrorMessage } from '../modules/errorMessageFormer';

export class EditCompanyForm extends Component {
    static displayName = EditCompanyForm.name;

    constructor(props) {
        super(props);

        this.state = {
            name: this.props.location.state.name,
            icon: JSON.stringify(base64ToArray(this.props.location.state.icon)),
            token: sessionStorage.getItem('token')
        };

        this.updateCompany = this.updateCompany.bind(this);
        this.handleName = this.handleName.bind(this);
        this.handleFileChange = this.handleFileChange.bind(this);
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
                    return formErrorMessage(response);
                }
            })
            .then(() => window.location.href = '/companies')
            .catch(error => alert(error.message));
    }

    handleName = event => {
        this.setState({
            name: event.target.value
        });
    }

    handleFileChange = event => {
        const reader = new FileReader();
        const fileByteArray = [];

        reader.readAsArrayBuffer(event.target.files[0]);
        reader.onloadend = evt => {
            if (evt.target.readyState === FileReader.DONE) {
                const arrayBuffer = evt.target.result;
                const array = new Uint8Array(arrayBuffer);

                for (let i = 0; i < array.length; i++) {
                    fileByteArray.push(array[i]);
                }

                this.setState({
                    icon: JSON.stringify(fileByteArray)
                });
            }
        }
    }

    render() {
        return <form onSubmit={this.updateCompany}>
            <div className="form-group my-3">
                <label htmlFor="name" className="control-label">Name:</label>
                <input id="name" className="form-control" type="text" value={this.state.name} onChange={this.handleName} /> 
            </div>
            <div className="form-group my-2">
                <label htmlFor="icon" className="form-label">Choose a picture:</label>
                <input id="icon" className="form-control" type="file" onChange={this.handleFileChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Update" />
        </form>;
    }
}